using Microsoft.EntityFrameworkCore;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Infrastructure;
using MobileTopup.Infrastructure.Repositories;

namespace MobileTopup.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext dbContext;
        public UserRepository(ApplicationContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public List<User> GetAll()
        {
            return dbContext.Users.AsNoTracking().ToList();
        }

        public User GetById(int id)
        {
            return dbContext.Users.Find(id);
        }

        public void Add(User user)
        {
            dbContext.Add(user);
            dbContext.SaveChanges();
        }
        public void Update(User user)
        {
            dbContext.Update(user);
            dbContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }

        public Task<IEnumerable<User>> GetAvailableUsersAsync()
        {
            var users = GetAll();

            return Task.FromResult<IEnumerable<User>>(users);
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
           var user = dbContext.Users
                .Include(u => u.Beneficiaries)
                .FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;    
        }

        
    }
}
