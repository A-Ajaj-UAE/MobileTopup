using MobileTopup.Contracts.Models;
using MobileTopup.Contracts.Requests;

namespace MobileTopup.API.Repositories
{
    public interface ITopupRepository
    {
        Task<List<TopupOption>> GetTopupOptionsAsync();
        Task<decimal> GetTotalTopupAmountAsync(string phone, int month, int year, string? beneficiaryPhoneNumber = null);
        Task PerformPopupTransactionAsync(User user, TopupRequest request);
    }
}
