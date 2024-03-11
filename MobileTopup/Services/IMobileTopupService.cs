using MobileTopup.Contracts.Models;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.API.Services
{
    public interface IMobileTopupService
    {
        /// <summary>
        /// get available topup options
        /// </summary>
        Task<IEnumerable<TopupOption>> GetAvailableTopupOptions();
        /// <summary>
        /// topup user beneficiary
        /// </summary>
        /// <param name="user"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TopupResponse> TopupBeneficiary(User user, TopupRequest request);
    }
}
