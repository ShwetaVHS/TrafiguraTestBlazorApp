using Dapper;
using System.Data;
using TrafiguraTestModels;
using TrafiguraTestWebApi.Contracts;
using WebAPI.Context;

namespace TrafiguraTestWebApi.Repository;

public class TransactionsRepository : ITransactionsRepository
{
    private readonly DapperContext _context;
    public TransactionsRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<TransactionDTO> GetPositions()
    {
        TransactionDTO transactionData = new TransactionDTO();
        var connection = _context.CreateConnection();
        using (connection)
        {
            connection.Open();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(ConcateAt(nameof(ErrCode)), 200, DbType.Byte, ParameterDirection.InputOutput);
            transactionData.Positions = (await connection.QueryAsync<TransactionModel>("usp_GetPositions", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false)).ToList();
            transactionData.ErrCode = (ErrCode)parameters.Get<byte>(ConcateAt(nameof(ErrCode)));
        }
        return transactionData;
    }

    public async Task SaveTransaction(TransactionDTO transactionData)
    {
        using (var connection = _context.CreateConnection())
        {
            connection.Open();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(ConcateAt(nameof(TransactionModel.TradeID)), transactionData.Transaction.TradeID, DbType.Int32, ParameterDirection.Input);
            parameters.Add(ConcateAt(nameof(TransactionModel.SecurityCode)), transactionData.Transaction.SecurityCode, DbType.String, ParameterDirection.Input);
            parameters.Add(ConcateAt(nameof(TransactionModel.Quantity)), transactionData.Transaction.Quantity, DbType.Int32, ParameterDirection.Input);
            parameters.Add(ConcateAt(nameof(TransactionModel.SubmitAction)), transactionData.Transaction.SubmitAction, DbType.Byte, ParameterDirection.Input);
            parameters.Add(ConcateAt(nameof(TransactionModel.IsActive)), transactionData.Transaction.IsActive, DbType.Boolean, ParameterDirection.Input);
            parameters.Add(ConcateAt(nameof(ErrCode)), 200, DbType.Byte, ParameterDirection.InputOutput);
            await connection.QueryAsync("usp_SaveTransaction", parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            transactionData.ErrCode = (ErrCode)parameters.Get<byte>(ConcateAt(nameof(ErrCode)));
        }
    }

    private string ConcateAt(string name)
    {
        return "@" + name;
    }
}
