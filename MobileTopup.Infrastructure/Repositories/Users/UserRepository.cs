using Microsoft.EntityFrameworkCore;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Infrastructure.Repositories;

namespace MobileTopup.API.Repositories
{
    public class UserRepository : BaseRepository , IUserRepository
    {
        private readonly DbContext _dbContext;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAvailableUsersAsync()
        {
            return await base.GetAllAsync<User>();
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
           var user = _dbContext.Set<User>().FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;    
        }
    }
}
