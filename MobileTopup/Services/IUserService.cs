using MobileTopup.Contracts.Domain.Entities;

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
        IEnumerable<Beneficiary> GetAvailableBeneficiaries(User user);
        /// <summary>
        /// add beneficiary to user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="beneficiary"></param>
        /// <returns></returns>
        Beneficiary AddBeneficiary(User user, Beneficiary beneficiary);
        /// <summary>
        /// get user balance
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Account> GetUserBalanceAsync(User user);
        /// <summary>
        /// debit user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<Account> DebitUserAsync(User user, decimal amount);
        /// <summary>
        /// credit user account
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<Account> CreditUserAsync(User user, decimal amount);
        /// <summary>
        /// get available users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAvailableUsersAsync();
    }
}