using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;

namespace MobileTopup.API.Repositories
{
    public interface ITopupRepository
    {
        Task<IEnumerable<TopupOption>> GetTopupOptionsAsync();
        Task<decimal> GetTotalTopupAmountAsync(string phone, int month, int year, string? beneficiaryPhoneNumber = null);
        Task PerformTopupTransactionAsync(User user, TopupRequest request);
    }
}
