CREATE TABLE dbo.tblTransactionHistory (
    TransactionID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	TradeID INT NOT NULL,
	[Version] INT NOT NULL,
    SecurityCode VARCHAR(50) NOT NULL,
	Quantity INT NOT NULL,
	SubmitAction SMALLINT,
	[Action] VARCHAR(50),
	LastModifiedON DATETIMEOFFSET(7)
);

CREATE TABLE dbo.tblTransactions (
    TransactionID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Version] INT NOT NULL,
    SecurityCode VARCHAR(50) NOT NULL,
	Quantity INT NOT NULL,
	LastModifiedON DATETIMEOFFSET(7)
);