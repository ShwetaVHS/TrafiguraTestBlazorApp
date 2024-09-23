using System.ComponentModel.DataAnnotations;

namespace TrafiguraTestModels;

public class TransactionModel
{
    [Required]
    public int? TradeID { get; set; }

    [Required]
    [StringLength(5, MinimumLength = 1,  ErrorMessage = "Quantity code value length should be between 0 to 5")]
    public string? SecurityCode { get; set; }

    [Required]
    [Range(0, 100, ErrorMessage = "Quantity should be between 0 to 100")]
    public int? Quantity { get; set; }

    //[Required]
    //[Range(1, 2, ErrorMessage = "Submitted Action should be between 1 to 2")]
    public int SubmitAction { get; set; }

    public bool IsActive { get; set; }
}