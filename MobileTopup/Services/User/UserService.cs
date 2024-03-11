using FluentValidation;
using MobileTopup.API.Repositories;
using MobileTopup.Contracts.Extensions;
using MobileTopup.Contracts.Models;
using Newtonsoft.Json;
using System.Text;

namespace MobileTopup.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IValidator<Beneficiary> beneficiaryValidator;
        private readonly IValidator<User> userValidator;
        private readonly HttpClient httpClient;
        private readonly ILogger<UserService> logger;
        private readonly string HOST = "https://localhost:7270/";

        public UserService(
            IUserRepository userRepository,
            IValidator<Beneficiary> beneficiaryValidator,
            IValidator<User> userValidator,
            IHttpClientFactory httpClientFactory,
            ILogger<UserService> logger
            
            )
        {
            this.userValidator = userValidator;
            this.userRepository = userRepository;
            this.beneficiaryValidator = beneficiaryValidator;
            this.logger = logger;
            this.httpClient = httpClientFactory.CreateClient();
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
           return userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public Beneficiary AddBeneficiary(User user, Beneficiary beneficiary)
        {
            ValidateUser(user);
            ValidateBeneficiary(beneficiary);

            //Add beneficiary to user
            user.AddBeneficiary(beneficiary);

           //revalidate user
            ValidateUser(user);

            return beneficiary;
        }

        public IEnumerable<Beneficiary> GetAvailableBeneficiaries(User user)
        {
            return user.Beneficiaries ?? new List<Beneficiary>();
        }

        public IEnumerable<User> GetAvailableUsers()
        {
           return userRepository.GetAvailableUsers();
        }

        #region Account from http 3rd party
        public async Task<Account> GetUserBalanceAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //read from http client api
            var response = await httpClient.GetAsync($"{HOST}api/v1/Account/{user.PhoneNumber}/balance");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Account>(content);
            }
            else
            {
                logger.LogError($"API error: Error getting balance, responseCode: {response.StatusCode}");
            }

            throw new Exception("Error getting balance");
        }

        public async Task<Account> DebitUserAsync(User user, decimal amount)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var payload = new StringContent(JsonConvert.SerializeObject(new { amount }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"{HOST}api/v1/Account/{user.PhoneNumber}/debit", payload);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Account>(content);
            }
            else
            {
                logger.LogError($"API error: Error debit balance, responseCode: {response.StatusCode}");
            }

            throw new Exception($"Error debiting balance for user {user.PhoneNumber}, API responseCode: {response.StatusCode}");
        }

        public async Task<Account> CreditUserAsync(User user, decimal amount)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var payload = new StringContent(JsonConvert.SerializeObject(new { amount }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"{HOST}api/v1/Account/{user.PhoneNumber}/credit", payload);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Account>(content);
            }
            else
            {
                logger.LogError($"API error: Error credit balance, responseCode: {response.StatusCode}");
            }

            throw new Exception($"Error credit balance for user {user.PhoneNumber}, API responseCode: {response.StatusCode}");
        }
        #endregion

        #region Private

        private void ValidateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var validationResult = userValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        private void ValidateBeneficiary(Beneficiary beneficiary)
        {
            if (beneficiary == null)
                throw new ArgumentNullException(nameof(beneficiary));

            var validationResult = beneficiaryValidator.Validate(beneficiary);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        #endregion
    }
}
