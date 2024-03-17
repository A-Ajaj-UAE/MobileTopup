using Microsoft.AspNetCore.Mvc;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;
using MobileTopup.Controllers;

namespace MobileTopup.API.Controllers
{
    /// <summary>
    /// Represents a RESTful service of accounts created in the same application for time constraints
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<TopupController> _logger;
        public AccountController(ILogger<TopupController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{phone}/balance")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Get([FromRoute] string phone)
        {
            try
            {
               return Ok(new Account { Balance = 5000 });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting balance");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{phone}/debit")]
        [ProducesResponseType(typeof(BalanceChangeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Debt([FromRoute] string phone, [FromBody] DebitRequest request)
        {
            try
            {
                //simulate credit account
                decimal balance = 5000;

                if (balance - request.Amount < 0)
                    throw new Exception("insufficient fund");

                var response = new BalanceChangeResponse { OldBalance = balance, NewBalance = balance - request.Amount };
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Debit Account");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{phone}/credit")]
        [ProducesResponseType(typeof(BalanceChangeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Credit([FromRoute] string phone, [FromBody] CreditRequest request)
        {
            try
            {
                //simulate credit account
                decimal balance = 5000;
                var response = new BalanceChangeResponse { OldBalance = balance, NewBalance = balance + request.Amount };
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Credit Account");
                return BadRequest(ex.Message);
            }
        }
    }
}
