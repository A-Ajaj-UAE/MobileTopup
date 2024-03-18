using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MobileTopup.API.Repositories;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Extensions;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;
using Newtonsoft.Json;
using System.Text;

namespace MobileTopup.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IValidator<AddBeneficiaryRequest> beneficiaryValidator;
        private readonly IValidator<User> userValidator;
        private readonly HttpClient httpClient;
        private readonly ILogger<UserService> logger;
        private readonly IMapper mapper;

        public UserService(
            IUserRepository userRepository,
            IValidator<AddBeneficiaryRequest> beneficiaryValidator,
            IValidator<User> userValidator,
            HttpClient httpClient,
            ILogger<UserService> logger,
            IMapper mapper
            
            )
        {
            this.userValidator = userValidator;
            this.userRepository = userRepository;
            this.beneficiaryValidator = beneficiaryValidator;
            this.logger = logger;
            this.mapper = mapper;
            this.httpClient = httpClient;
        }

        public UserResponse AddUser(CreateUserRequest request)
        {
            var user = new User()
            {
                TopupHistories = new List<TopupHistory>(),
                Account = new Account() { Balance = 0},
                PhoneNumber = request.PhoneNumber,
                Name = request.Name,
                IsVerified = true,
                Remark = request.Remark
            };

            userRepository.Add(user);

            return mapper.Map<UserResponse>(user);
        }

        public User GetUserByPhoneNumber(string phoneNumber)
        {
           return userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public BeneficiaryResponse AddBeneficiary(User user, AddBeneficiaryRequest request)
        {
            ValidateBeneficiary(request);

            //Add beneficiary to user
            var beneficiary = new Beneficiary()
            {
                NickName = request.NickName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.IsActive
            };
            user.AddBeneficiary(beneficiary);

           //revalidate user
            ValidateUser(user);

            userRepository.Update(user);

            return mapper.Map<BeneficiaryResponse>(beneficiary);
        }

        public IEnumerable<BeneficiaryResponse> GetAvailableBeneficiaries(User user)
        {
            var beneficiaries =  user.Beneficiaries ?? new List<Beneficiary>();

            return mapper.Map<IEnumerable<BeneficiaryResponse>>(beneficiaries);
        }

        public async Task<IEnumerable<UserResponse>> GetAvailableUsersAsync()
        {
           var users = await userRepository.GetAvailableUsersAsync();

           return mapper.Map<IEnumerable<UserResponse>>(users);
        }

        #region Account from http 3rd party
        public async Task<AccountResponse> GetUserBalanceAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //read from http client api
            var response = await httpClient.GetAsync($"api/v1/Account/{user.PhoneNumber}/balance");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<Account>(content);

                if (account == null)
                {
                    logger.LogError($"Error deserialize account object, User {user.PhoneNumber}");
                    throw new Exception("Error deserialize account object");
                }

                return mapper.Map<AccountResponse>(account);
            }
            else
            {
                logger.LogError($"API error: Error getting balance, responseCode: {response.StatusCode}");
            }

            throw new Exception("Error getting balance");
        }

        public async Task<BalanceChangeResponse> DebitUserAsync(User user, decimal amount)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var payload = new StringContent(JsonConvert.SerializeObject(new CreditRequest { Amount = amount }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"api/v1/Account/{user.PhoneNumber}/debit", payload);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var balanceChangeResponse = JsonConvert.DeserializeObject<BalanceChangeResponse>(content);

                if (balanceChangeResponse == null)
                {
                    logger.LogError($"Error deserialize balance change response object, User {user.PhoneNumber}");
                    throw new Exception("Error deserialize balance change response object");    
                }

                return balanceChangeResponse;
            }
            else
            {
                logger.LogError($"API error: Error debit balance, responseCode: {response.StatusCode}");
            }

            throw new Exception($"Error debiting balance for user {user.PhoneNumber}, API responseCode: {response.StatusCode}");
        }

        public async Task<BalanceChangeResponse> CreditUserAsync(User user, decimal amount)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var payload = new StringContent(JsonConvert.SerializeObject(new DebitRequest{ Amount= amount  }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"api/v1/Account/{user.PhoneNumber}/credit", payload);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var balanceChangeResponse = JsonConvert.DeserializeObject<BalanceChangeResponse>(content);

                if (balanceChangeResponse == null)
                {
                    logger.LogError($"Error deserialize balance change response object, User {user.PhoneNumber}");
                    throw new Exception("Error deserialize balance change response object");
                }

                return balanceChangeResponse;
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

        private void ValidateBeneficiary(AddBeneficiaryRequest beneficiary)
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
