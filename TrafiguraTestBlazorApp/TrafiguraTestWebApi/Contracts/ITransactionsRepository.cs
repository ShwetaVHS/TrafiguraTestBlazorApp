using TrafiguraTestModels;

namespace TrafiguraTestWebApi.Contracts;

public interface ITransactionsRepository
{
    public Task<TransactionDTO> GetPositions();
    public Task SaveTransaction(TransactionDTO transactionData);
}
