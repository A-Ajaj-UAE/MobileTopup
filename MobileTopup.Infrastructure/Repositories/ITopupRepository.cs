using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Infrastructure.Repositories;

namespace MobileTopup.API.Repositories
{
    public interface ITopupRepository : IBaseRepository<TopupHistory>
    {
        Task<List<TopupOption>> GetTopupOptionsAsync();
        decimal GetTotalTopupAmountAsync(string phone, int month, int year, string? beneficiaryPhoneNumber = null);
        Task PerformTopupTransactionAsync(User user, TopupRequest request);
    }
}
