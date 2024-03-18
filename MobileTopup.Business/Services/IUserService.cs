using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Response;

namespace MobileTopup.API.Services
{
    public interface IUserService
    {
        /// <summary>
        /// get user by phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        User GetUserByPhoneNumber(string phoneNumber);
        /// <summary>
        /// get available beneficiaries for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IEnumerable<BeneficiaryResponse> GetAvailableBeneficiaries(User user);
        /// <summary>
        /// add beneficiary to user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="beneficiary"></param>
        /// <returns></returns>
        BeneficiaryResponse AddBeneficiary(User user, AddBeneficiaryRequest beneficiary);
        /// <summary>
        /// get user balance
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<AccountResponse> GetUserBalanceAsync(User user);
        /// <summary>
        /// debit user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<BalanceChangeResponse> DebitUserAsync(User user, decimal amount);
        /// <summary>
        /// credit user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<BalanceChangeResponse> CreditUserAsync(User user, decimal amount);
        /// <summary>
        /// get available users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserResponse>> GetAvailableUsersAsync();
        /// <summary>
        /// add user
        /// </summary>
        /// <param name="user"></param>
        UserResponse AddUser(CreateUserRequest user);
    }
}