using MobileTopup.Contracts.Models;
using MobileTopup.Contracts.Requests;

namespace MobileTopup.API.Repositories
{
    public class TopupRepository : ITopupRepository
    {
        public async Task<List<TopupOption>> GetTopupOptionsAsync()
        {
            // simulate getting topup options from a database
            return new List<TopupOption>
                {
                    new TopupOption
                    {
                        Name = "AED5",
                        Amount = 5
                    },
                    new TopupOption
                    {
                        Name = "AED20",
                        Amount = 20
                    },
                    new TopupOption
                    {
                        Name = "AED30",
                        Amount = 30
                    },
                    new TopupOption
                    {
                        Name = "AED50",
                        Amount = 50
                    },
                    new TopupOption
                    {
                        Name = "AED75",
                        Amount = 75
                    },
                    new TopupOption
                    {
                        Name = "AED100",
                        Amount = 100
                    }
                };
        }

        public async Task<decimal> GetTotalTopupAmountAsync(string phone, int month, int year, string? beneficiryPhoneNumber = null)
        {
           // simulate getting total topup amount from a database
           // benefitiaryPhoneNumber is optional and will be used to get the total topup amount for a specific beneficiary
           Random random = new Random();
           // generate a random number between 1 and 3000
           decimal total = random.Next(1, 3001);

           return total;
        }

        public Task PerformPopupTransactionAsync(User user, TopupRequest request)
        {
            // simulate performing topup transaction
            return Task.CompletedTask;
        }
    }
}
