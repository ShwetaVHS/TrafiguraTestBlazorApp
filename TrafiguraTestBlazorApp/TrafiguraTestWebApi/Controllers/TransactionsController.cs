using Microsoft.AspNetCore.Mvc;
using TrafiguraTestModels;
using TrafiguraTestWebApi.Contracts;

namespace TrafiguraTestWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionsRepository _transactionRepo;
    public TransactionsController(ITransactionsRepository transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    [Route("GetPositions")]
    [HttpGet]
    public async Task<IActionResult> GetPositions()
    {
        TransactionDTO positionData = null;
        try
        {
            positionData = await _transactionRepo.GetPositions();
            if (positionData.ErrCode == ErrCode.OK)
            {
                if (positionData.Positions == null || positionData.Positions.Count < 1)
                {
                    positionData.ErrCode = ErrCode.NoDataFound;
                }
                return Ok(positionData);
            }
            else
            {
                return StatusCode((int)positionData.ErrCode, GenericMothod.GetErrorMessage((int)positionData.ErrCode));
            }
        }
        catch (Exception ex)
        {
            positionData ??= new TransactionDTO();
            positionData.ErrCode = ErrCode.InternalServerError;
            return StatusCode((int)positionData.ErrCode, ex.Message);
        }
    }

    [Route("SaveTransactions")]
    [HttpPost]
    public async Task<IActionResult> SaveTransactions([FromBody] TransactionDTO transactionData)
    {
        try
        {
            if (transactionData.Transaction == null
                || string.IsNullOrWhiteSpace(transactionData.Transaction.SecurityCode)
                || transactionData.Transaction.TradeID < 1
                || transactionData.Transaction.SubmitAction > 2
                || transactionData.Transaction.SubmitAction < 1)
            {
                transactionData.ErrCode = ErrCode.InvalidData;
                return StatusCode((int)transactionData.ErrCode, GenericMothod.GetErrorMessage((int)transactionData.ErrCode));
            }
            await _transactionRepo.SaveTransaction(transactionData);
            if (transactionData.ErrCode == ErrCode.OK)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)transactionData.ErrCode, GenericMothod.GetErrorMessage((int)transactionData.ErrCode));
            }
        }
        catch (Exception ex)
        {
            transactionData.ErrCode = ErrCode.InternalServerError;
            return StatusCode((int)transactionData.ErrCode, ex.Message);
        }
    }
}
