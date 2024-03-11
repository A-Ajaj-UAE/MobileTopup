using MobileTopup.Contracts.Models;

namespace MobileTopup.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> GetAvailableUsers()
        {
            //simulate users in db
            return new List<User>
            {
                new User
                {
                    PhoneNumber = "1234567890",
                    Name = "John Doe",
                    Remark = "This is active user for active beneficiry",
                    IsVerified = true,
                    Beneficiaries = new List<Beneficiary>
                    {
                        new Beneficiary
                        {
                            NickName = "Jane Doe",
                            Phone = "1234567890",
                            IsActive = true
                        }
                    }
                },
                new User
                {
                    PhoneNumber = "1234567891",
                    Name = "Max Doe",
                    Remark = "This is inactive user for inactive beneficiry",
                    IsVerified = false,
                    Beneficiaries = new List<Beneficiary>
                    {
                        new Beneficiary
                        {
                            NickName = "John Doe",
                            Phone = "0987654321",
                            IsActive = false
                        }
                    }
                },
                new User
                {
                    PhoneNumber = "1234567892",
                    Name = "Max Doe",
                    Remark = "This is active user for max active beneficiry",
                    IsVerified = true,
                    Beneficiaries = new List<Beneficiary>
                    {
                        new Beneficiary
                        {
                            NickName = "John Doe",
                            Phone = "0987654321",
                            IsActive = true
                        },
                         new Beneficiary
                        {
                            NickName = "John Doe",
                            Phone = "0987654321",
                            IsActive = true
                        },
                          new Beneficiary
                        {
                            NickName = "John Doe",
                            Phone = "0987654321",
                            IsActive = true
                        }, new Beneficiary
                        {
                            NickName = "John Doe",
                            Phone = "0987654321",
                            IsActive = true
                        }, new Beneficiary
                        {
                            NickName = "John Doe",
                            Phone = "0987654321",
                            IsActive = true
                        }
                    }
                }
            };
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
            //simulate users from db;
            var users = GetAvailableUsers();

            var user = users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;    
        }
    }
}
