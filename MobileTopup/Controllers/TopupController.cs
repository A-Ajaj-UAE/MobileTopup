using Microsoft.AspNetCore.Mvc;
using MobileTopup.API.Services;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TopupController : ControllerBase
    {
        private readonly ILogger<TopupController> _logger;
        private readonly IUserService userService;
        private readonly IMobileTopupService topupService;

        public TopupController(ILogger<TopupController> logger, IUserService userService, IMobileTopupService topupService)
        {
            _logger = logger;
            this.userService = userService;
            this.topupService = topupService;
        }

        /// <summary>
        /// Get list of beneficiaries for a user
        /// </summary>
        /// <param name="phone"></param>
        /// <response code="200">List of beneficiaries</response>
        [HttpGet("{phone}/beneficiaries")]
        [ProducesResponseType(typeof(IEnumerable<Beneficiary>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Beneficiary>> Get([FromRoute] string phone)
        {
            try
            {
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    return NotFound();

                var beneficiaries = userService.GetAvailableBeneficiaries(user);

                return Ok(beneficiaries);
            }
            catch(KeyNotFoundException ex)
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

        /// <summary>
        /// Add a beneficiary to a user
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="beneficiary"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Beneficiary), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("{phone}/add-beneficiary")]
        public ActionResult<Beneficiary> Add([FromRoute] string phone, Beneficiary beneficiary)
        {
            try
            {
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    return NotFound();

                var response = userService.AddBeneficiary(user, beneficiary);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// fetch available topup options
        /// </summary>
        /// <returns></returns>
        [HttpGet("topup-options")]
        [ProducesResponseType(typeof(IEnumerable<TopupOption>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<TopupOption>> AvailableOptions()
        {
            try
            {
                var response = topupService.GetAvailableTopupOptions();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// fetch available topup options
        /// </summary>
        /// <returns></returns>
        [HttpPost("{phone}/topup-beneficiary")]
        [ProducesResponseType(typeof(IEnumerable<TopupResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<TopupResponse> TopupAccount([FromRoute] string phone, TopupRequest request)
        {
            try
            {
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    return NotFound();

                var response = topupService.TopupBeneficiary(user, request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// fetch available topup options
        /// </summary>
        /// <returns></returns>
        [HttpGet("{phone}/get-balance")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public  async Task<ActionResult<Account>> GetAccountAsync([FromRoute] string phone)
        {
            try
            {
                var user = userService.GetUserByPhoneNumber(phone);

                if (user == null)
                    return NotFound();

                var response = await userService.GetUserBalanceAsync(user);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
