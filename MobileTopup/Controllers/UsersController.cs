using Microsoft.AspNetCore.Mvc;
using MobileTopup.API.Services;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Response;

namespace MobileTopup.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<TopupController> _logger;
        private readonly IUserService userService;
        public UsersController(ILogger<TopupController> logger, IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        /// <summary>
        /// Get list of beneficiaries for a user
        /// </summary>
        /// <param name="phone"></param>
        /// <response code="200">List of beneficiaries</response>
        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Beneficiary>> Get()
        {
            try
            {
                var users = userService.GetAvailableUsersAsync();

                return Ok(users);
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Users not found");
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
