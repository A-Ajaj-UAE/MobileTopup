using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Infrastructure.Repositories;

namespace MobileTopup.API.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Account GetBalance(User user);
        void AddTransaction(Transaction credit);
        void UpdateAccount(Account account);
    }
}
