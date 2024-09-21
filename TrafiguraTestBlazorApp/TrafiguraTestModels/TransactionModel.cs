using System.ComponentModel.DataAnnotations;

namespace TrafiguraTestModels;

public class TransactionModel
{
    //public int TransactionID { get; set; }
    [Required]
    public int? TradeID { get; set; }
    //public int Version { get; set; }
    [Required]
    public string SecurityCode { get; set; }
    [Required]
    public int? Quantity { get; set; }
    //public int Action { get; set; }
    public int SubmitAction { get; set; }
    public bool IsActive { get; set; }
}