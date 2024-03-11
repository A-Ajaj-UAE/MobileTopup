using Microsoft.AspNetCore.Mvc;
using MobileTopup.Contracts.Models;
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
        [ProducesResponseType(typeof(IEnumerable<Beneficiary>), StatusCodes.Status200OK)]
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
                _logger.LogError(ex, "Error getting beneficiaries");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{phone}/debit")]
        [ProducesResponseType(typeof(IEnumerable<Beneficiary>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Debt([FromRoute] string phone)
        {
            try
            {
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting beneficiaries");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{phone}/credit")]
        [ProducesResponseType(typeof(IEnumerable<Beneficiary>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Credit([FromRoute] string phone)
        {
            try
            {
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting beneficiaries");
                return BadRequest(ex.Message);
            }
        }
    }
}
