using Microsoft.EntityFrameworkCore;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Infrastructure.Repositories;

namespace MobileTopup.API.Repositories
{
    public class TopupRepository : BaseRepository, ITopupRepository
    {
        private readonly DbContext _dbContext;
        public TopupRepository(DbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<TopupOption>> GetTopupOptionsAsync()
        {
            return await base.GetAllAsync<TopupOption>();
        }

        public async Task<decimal> GetTotalTopupAmountAsync(string phone, int month, int year, string? beneficiryPhoneNumber = null)
        {
            var query = _dbContext.Set<TopupHistory>().AsQueryable();

            if (phone == null)
            {
                throw new ArgumentNullException(nameof(phone));
            }

            query = query.Where(b => b.User.PhoneNumber == phone);

            query = query.Where(b => b.Date.Month == month && b.Date.Year == year);

            if (beneficiryPhoneNumber != null)
            {
                query = query.Where(b => b.PhoneNumber == beneficiryPhoneNumber);
            }

            return await query.SumAsync(b => b.Amount);
        }

        public Task PerformTopupTransactionAsync(User user, TopupRequest request)
        {
            var topupHistory = new TopupHistory(request.PhoneNumber, request.Amount, DateTime.UtcNow)
            {
                UserId = user.Id
            };

            base.Add(topupHistory);

            return Task.CompletedTask;
        }
    }
}
