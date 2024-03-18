using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.API.Services
{
    public interface IAccountService
    {
        BalanceChangeResponse Credit(User user, CreditRequest creditRequest);
        BalanceChangeResponse Debit(User user, DebitRequest debitRequest);
        AccountResponse GetBalance(User user);
    }
}