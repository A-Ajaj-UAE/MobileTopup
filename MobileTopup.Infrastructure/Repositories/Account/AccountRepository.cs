using Microsoft.EntityFrameworkCore;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Infrastructure;

namespace MobileTopup.API.Repositories
{
    public class AccountRepository :  IAccountRepository
    {
        private readonly ApplicationContext dbContext;

        public AccountRepository(ApplicationContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public void AddTransaction(Transaction credit)
        {
            dbContext.Transactions.Add(credit);
            dbContext.SaveChanges();
        }

        public void UpdateAccount(Account account)
        {
            dbContext.Accounts.Update(account);
            dbContext.SaveChanges();
        }

        public Account GetBalance(User user)
        {
            //get account from user
            return dbContext.Accounts.FirstOrDefault(a => a.UserId.Equals(user.Id));
        }

        public void Add(Account entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Account entity)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAll()
        {
            throw new NotImplementedException();
        }

        public Account GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}
