using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Infrastructure.Repositories;

namespace MobileTopup.API.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAvailableUsersAsync();
        User GetUserByPhoneNumber(string phoneNumber);
    }
}
