namespace TrafiguraTestModels;

public class TransactionDTO
{
    public TransactionModel Transaction{ get; set; }

    public List<TransactionModel> Positions { get; set; }
    public ErrCode ErrCode { get; set; }
}