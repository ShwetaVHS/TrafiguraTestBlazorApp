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
        try
        {
            var positionData = await _transactionRepo.GetPositions();
            if (positionData.ErrCode == ErrCode.OK)
            {
                return Ok(positionData);
                //if (positionData.Positions?.Count > 0)
                //{
                //    return Ok(positionData.Positions);
                //}
                //return NotFound("No Data Found!!!");
            }
            else
            {
                return StatusCode((int)positionData.ErrCode, "Error While Retrieving Records");
            }
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    [Route("SaveTransactions")]
    [HttpPost]
    public async Task<IActionResult> SaveTransactions([FromBody] TransactionDTO transactionData)
    {
        try
        {
            await _transactionRepo.SaveTransaction(transactionData);
            if (transactionData.ErrCode == ErrCode.OK)
            {
                return Ok();
            }
            else
            {
                return StatusCode((int)transactionData.ErrCode, "Error While Saving Records");
            }
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }
}
