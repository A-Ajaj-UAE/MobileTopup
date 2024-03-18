using AutoMapper;
using Microsoft.Extensions.Logging;
using MobileTopup.API.Repositories;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ILogger<UserService> logger;
        private readonly IMapper mapper;

        public AccountService(
            IAccountRepository accountRepository,
            ILogger<UserService> logger,
            IMapper mapper
            )
        {
            this.accountRepository = accountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public AccountResponse GetBalance(User user)
        {
            var account = accountRepository.GetBalance(user);

            if (account == null)
            {
                logger.LogError($"Account not found user {user.Name}");
                throw new NotImplementedException("Account not found");
            }

            return mapper.Map<AccountResponse>(account);
        }

        public BalanceChangeResponse Credit(User user, CreditRequest creditRequest)
        {
            var account = accountRepository.GetBalance(user);

            var oldBalance = account.Balance;

            var credit = new Transaction
            {
                Amount = creditRequest.Amount,
                Date = DateTime.UtcNow,
                PhoneNumber = user.PhoneNumber,
                AccountId = account.Id
            };

            account.Balance += creditRequest.Amount;

            accountRepository.UpdateAccount(account);

            accountRepository.AddTransaction(credit);

            var result = new BalanceChangeResponse { OldBalance = oldBalance, NewBalance = account.Balance };

            return result;

        }

        public BalanceChangeResponse Debit(User user, DebitRequest debitRequest)
        {
            var account = accountRepository.GetBalance(user);

            if (account.Balance - debitRequest.Amount < 0)
                throw new Exception("insufficient fund");

            var oldBalance = account.Balance;

            var debit = new Transaction
            {
                Amount = debitRequest.Amount,
                Date = DateTime.UtcNow,
                PhoneNumber = user.PhoneNumber,
                AccountId = account.Id
            };

            account.Balance -= debitRequest.Amount;

            accountRepository.UpdateAccount(account);

            accountRepository.AddTransaction(debit);

            var result = new BalanceChangeResponse { OldBalance = oldBalance, NewBalance = account.Balance };

            return result;
        }
    }
}
