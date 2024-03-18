using Microsoft.AspNetCore.Mvc;
using MobileTopup.API.Services;
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
        private readonly IAccountService accountService;
        private readonly IUserService userService;

        public AccountController(ILogger<TopupController> logger,
            IAccountService accountService,
            IUserService userService)
        {
            _logger = logger;
            this.accountService = accountService;
            this.userService = userService;
        }

        [HttpGet("{phone}/balance")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult Get([FromRoute] string phone)
        {
            try
            {
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                var account = accountService.GetBalance(user);

                return Ok(account);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                //generic handler will return the message
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Credit Account");
                //generic handler will return the message
                throw ex;
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
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                var response = accountService.Debit(user, request);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                //generic handler will return the message
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Credit Account");
                //generic handler will return the message
                throw ex;
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
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    throw new KeyNotFoundException("User not found");

                var response = accountService.Credit(user, request);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "User not found");
                //generic handler will return the message
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Credit Account");
                //generic handler will return the message
                throw ex;
            }
        }
    }
}
