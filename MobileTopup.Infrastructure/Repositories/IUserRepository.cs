using MobileTopup.Contracts.Models;

namespace MobileTopup.API.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAvailableUsers();
        User GetUserByPhoneNumber(string phoneNumber);
    }
}
