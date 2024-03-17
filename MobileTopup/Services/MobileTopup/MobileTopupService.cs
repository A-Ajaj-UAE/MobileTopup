using Microsoft.Extensions.Options;
using MobileTopup.API.Repositories;
using MobileTopup.API.Settings;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.API.Services
{
    public class MobileTopupService : IMobileTopupService
    {
        private readonly IUserService _userService;
        private readonly ITopupRepository _topupRepository;
        private readonly TopupSettings _topupSettings;

        //the transaction fee is 1 as per the requirement
        private readonly decimal TransactionFee;
        private readonly decimal UnverifiedMaxBeneficiryAmount;
        private readonly decimal VerifiedMaxBeneficiryAmount;
        private readonly decimal MaxBeneficiriresAmount;

        public MobileTopupService(
            IUserService userService,
            ITopupRepository settingRepository,
            IOptions<TopupSettings> topupSettings
            )
        {
            _userService = userService;
            _topupRepository = settingRepository;
            _topupSettings = topupSettings.Value;
            TransactionFee = _topupSettings.TransactionFee;
            UnverifiedMaxBeneficiryAmount = _topupSettings.UnverifiedMaxBeneficiryAmount;
            VerifiedMaxBeneficiryAmount = _topupSettings.VerifiedMaxBeneficiryAmount;
            MaxBeneficiriresAmount = _topupSettings.MaxBeneficiriresAmount;

        }

        #region public
        public async Task<IEnumerable<TopupOption>> GetAvailableTopupOptions()
        {
            var options = await _topupRepository.GetTopupOptionsAsync();

            if (options is null)
                throw new Exception("Top up options not found");

            return options;
        }

        public async Task<TopupResponse> TopupBeneficiary(User user, TopupRequest request)
        {
            // Step 1: Check if the total top-up amount for all beneficiaries exceeds the limit
            decimal totalTopupAmount = await GetTotalTopupAmountCurrentMonthAsync(user);
            ValidateMaxTopupAmountAllBeneficiaries(request.Amount, totalTopupAmount);

            // Step 2: Determine the top-up amount based on the user's verification status
            var totalBeneficiaryTopupAmount = await GetTotalTopupAmountCurrentMonthAsync(user, request.PhoneNumber);
            ValidateMaxTopupAmount(request.Amount, user.IsVerified, totalBeneficiaryTopupAmount);

            // Step 3: Get the user's balance from the external HTTP service
            var account = await GetUserBalanceAsync(user);

            // Step 4: Check if the user's balance is sufficient for the top-up
            var topupAmount = request.Amount + TransactionFee;
            ValidateSufficientBalance(topupAmount, account.Balance);

            // Step 5: Debit the user's balance
            // will throw exception for any error
            await DebitUserBalanceAsync(user, topupAmount);

            // Step 6: Perform the top-up transaction
            var topupResponse = await PerformTopupTransactionAsync(user, request);

            // Step 7: Return the top-up result
            return topupResponse;
        }
        #endregion

        #region validation  
        public void ValidateSufficientBalance(decimal topupAmount, decimal balance)
        {
            if (balance < topupAmount)
                throw new Exception("Insufficient balance");
        }

        public void ValidateMaxTopupAmountAllBeneficiaries(decimal amount, decimal totalBeneficiaryTopupAmount)
        {
            if (amount + totalBeneficiaryTopupAmount > MaxBeneficiriresAmount)
            {
                throw new Exception($"Total top-up amount for all beneficiaries cannot exceed {MaxBeneficiriresAmount} AED");
            }
        }

        public void ValidateMaxTopupAmount(decimal amount, bool isVerified, decimal totalAmount)
        {
            if (!isVerified && amount + totalAmount > UnverifiedMaxBeneficiryAmount)
            {
                throw new Exception($"Unverified user's beneficiary cannot receive more than {UnverifiedMaxBeneficiryAmount} AED");
            }

            if (isVerified && amount + totalAmount > VerifiedMaxBeneficiryAmount)
            {
                throw new Exception($"Verified user's beneficiary cannot receive more than {VerifiedMaxBeneficiryAmount} AED");
            }
        }

        #endregion

        #region private
        private async Task<decimal> GetTotalTopupAmountCurrentMonthAsync(User user, string? phoneNumner = null)
        {
            // assume the client is in the same time zone as the server
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var total = await _topupRepository.GetTotalTopupAmountAsync(user.PhoneNumber, month, year, phoneNumner);

            return total;
        }
        private async Task<Account> GetUserBalanceAsync(User user)
        {
            //Implement the logic to retrieve the user's balance from the external HTTP service
            var account = await _userService.GetUserBalanceAsync(user);

            return account;
        }
        private async Task<Account> DebitUserBalanceAsync(User user, decimal amount)
        {
            var account = await _userService.DebitUserAsync(user, amount);

            return account;
        }
        private async Task<TopupResponse> PerformTopupTransactionAsync(User user, TopupRequest request)
        {
            try
            {
                await _topupRepository.PerformTopupTransactionAsync(user, request);

                return new TopupResponse
                {
                    PhoneNumber = request.PhoneNumber,
                    Amount = request.Amount,
                    Date = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error performing top-up transaction", ex);
            }
        }
        #endregion
    }
}
