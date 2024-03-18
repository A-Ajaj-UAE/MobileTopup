using Microsoft.EntityFrameworkCore;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Infrastructure;

namespace MobileTopup.API.Repositories
{
    public class TopupRepository :  ITopupRepository
    {
        private readonly ApplicationContext _dbContext;
        public TopupRepository(ApplicationContext dbContext) 
        {
            this._dbContext = dbContext;
        }

        public void Add(TopupHistory entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TopupHistory entity)
        {
            throw new NotImplementedException();
        }

        public List<TopupHistory> GetAll()
        {
            throw new NotImplementedException();
        }

        public TopupHistory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TopupOption>> GetTopupOptionsAsync()
        {
            return await _dbContext.TopupOptions.AsNoTracking().ToListAsync();
        }

        public decimal GetTotalTopupAmountAsync(string phone, int month, int year, string? beneficiryPhoneNumber = null)
        {
            if (phone == null)
            {
                throw new ArgumentNullException(nameof(phone));
            }

            var query = _dbContext.TopupHistories.AsQueryable();

            query = query.Where(b => b.User.PhoneNumber == phone);

            query = query.Where(b => b.Date.Month == month && b.Date.Year == year);

            if (beneficiryPhoneNumber != null)
            {
                query = query.Where(b => b.PhoneNumber == beneficiryPhoneNumber);
            }

            return  query.Sum(b => b.Amount);
        }

        public Task PerformTopupTransactionAsync(User user, TopupRequest request)
        {
            var topupHistory = new TopupHistory(request.PhoneNumber, request.Amount, DateTime.UtcNow)
            {
                UserId = user.Id
            };

            _dbContext.TopupHistories.Add(topupHistory);
            _dbContext.SaveChanges();

            return Task.CompletedTask;
        }

        public void Update(TopupHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}
