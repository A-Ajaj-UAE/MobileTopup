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

        public TopupResponse TopupBeneficiary(User user, TopupRequest request)
        {
            //validate request belong to the available topup options
            ValidateTopupAmount(request).Wait();

            //validate topup beneficiary is availabe in user beneficiaries and its active
            ValidateBeneficiaryIsActive(user, request);

            // Step 1: Check if the total top-up amount for all beneficiaries exceeds the limit
            decimal totalTopupAmount = GetTotalTopupAmountCurrentMonthAsync(user);
            ValidateMaxTopupAmountAllBeneficiaries(request.Amount, totalTopupAmount);

            // Step 2: Determine the top-up amount based on the user's verification status
            var totalBeneficiaryTopupAmount = GetTotalTopupAmountCurrentMonthAsync(user, request.PhoneNumber);
            ValidateMaxTopupAmount(request.Amount, user.IsVerified, totalBeneficiaryTopupAmount);

            // Step 3: Get the user's balance from the external HTTP service
            var account =  GetUserBalanceAsync(user).Result;

            // Step 4: Check if the user's balance is sufficient for the top-up
            var topupAmount = request.Amount + TransactionFee;
            ValidateSufficientBalance(topupAmount, account.Balance);

            // Step 5: Debit the user's balance
            // will throw exception for any error
            DebitUserBalanceAsync(user, topupAmount).Wait();

            // Step 6: Perform the top-up transaction
            _topupRepository.PerformTopupTransactionAsync(user, request).Wait();

            // Step 7: Return the top-up result
            return new TopupResponse { Amount = request.Amount , PhoneNumber = request.PhoneNumber, Date = DateTime.Now };
            
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

        public void ValidateBeneficiaryIsActive(User user, TopupRequest request)
        {
            var beneficiaries = _userService.GetAvailableBeneficiaries(user);
            var beneficiary = beneficiaries.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber && x.IsActive);
            if (beneficiary == null)
                throw new Exception("Beneficiary not found or not active");
        }

        public async Task ValidateTopupAmount(TopupRequest request)
        {
            var topupOptions = await GetAvailableTopupOptions();
            if (!topupOptions.Any(x => x.Amount == request.Amount))
                throw new Exception("Invalid top-up amount");
        }

        #endregion

        #region private
        private decimal GetTotalTopupAmountCurrentMonthAsync(User user, string? phoneNumner = null)
        {
            // assume the client is in the same time zone as the server
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var total = _topupRepository.GetTotalTopupAmountAsync(user.PhoneNumber, month, year, phoneNumner);

            return total;
        }
        private async Task<AccountResponse> GetUserBalanceAsync(User user)
        {
            //Implement the logic to retrieve the user's balance from the external HTTP service
            var account = await _userService.GetUserBalanceAsync(user);

            return account;
        }
        private async Task<BalanceChangeResponse> DebitUserBalanceAsync(User user, decimal amount)
        {
            var balanceChangeResponse = await _userService.DebitUserAsync(user, amount);

            return balanceChangeResponse;
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
