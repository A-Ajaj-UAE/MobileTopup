using MobileTopup.Contracts.Domain.Entities;

namespace MobileTopup.API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAvailableUsersAsync();
        User GetUserByPhoneNumber(string phoneNumber);
    }
}
