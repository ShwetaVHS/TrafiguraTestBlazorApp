CREATE OR ALTER PROCEDURE dbo.usp_SaveTransaction
    @tradeID INT,
	@securityCode VARCHAR(50),
	@quantity INT,
	@submitAction SMALLINT, --1: Buy, 2: Sell
	@isActive BIT,
	@errCode SMALLINT OUTPUT
AS
SET NOCOUNT ON
SET @errCode = 200;
BEGIN
	BEGIN TRY 
		DECLARE @existingVersion INT = 0,
			@existingQuantity INT = 0,
			@transactionID INT = 0,
			@action VARCHAR(50);

		SELECT TOP 1 @transactionID= TransactionID, @existingVersion = [Version], @existingQuantity = Quantity 
		FROM dbo.tblTransactions 
		WHERE SecurityCode = @securityCode
		ORDER BY LastModifiedON DESC;

		IF @transactionID > 0
		BEGIN
			SET @existingVersion = @existingVersion + 1;
			IF @isActive = 0
			BEGIN
				-- Cancel
				SET @existingQuantity = 0;
				SET @action = 'CANCEL';
			END
			--Note: @submitAction = 1: Buy, 2: Sell
			ELSE IF @submitAction = 1
			BEGIN
				SET @existingQuantity = @quantity;
				--SET @existingQuantity = @existingQuantity + @quantity;
				SET @action = 'UPDATE';
			END
			ELSE
			BEGIN
				SET @existingQuantity = @existingQuantity - @quantity;
				SET @action = 'UPDATE';
			END

			--UPDATE
			UPDATE dbo.tblTransactions 
			SET [Version] = @existingVersion, Quantity = @existingQuantity, LastModifiedON = GETUTCDATE() 
			WHERE TransactionID = @transactionID;
		END
		ELSE IF @isActive = 1
		BEGIN
			SET @existingVersion = 1;
			SET @existingQuantity = @quantity;
			SET @action = 'INSERT';

			--INSERT
			INSERT INTO dbo.tblTransactions([Version], SecurityCode, Quantity, LastModifiedON)
			VALUES(@existingVersion, @securityCode, @existingQuantity, GETUTCDATE());
		END
		ELSE
		BEGIN 
			SET @errCode = 591;
			RETURN 
		END

		IF(@action = 'UPDATE' AND NOT EXISTS(SELECT 1 FROM dbo.tblTransactionHistory WHERE SecurityCode = @securityCode AND TradeID = @tradeID))
		BEGIN
			SET @action = 'INSERT';
		END

		INSERT INTO dbo.tblTransactionHistory (TradeID, [Version], SecurityCode, Quantity, SubmitAction, [Action], LastModifiedON)
		VALUES(@tradeID, @existingVersion, @securityCode, @quantity, @submitAction, @action, GETUTCDATE());

	END TRY
	BEGIN CATCH
		SET @errCode = 500;
	END CATCH
END