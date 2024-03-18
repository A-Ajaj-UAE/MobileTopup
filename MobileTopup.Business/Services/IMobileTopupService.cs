using MobileTopup.Contracts.Domain.Entities;
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
        TopupResponse TopupBeneficiary(User user, TopupRequest request);
    }
}
