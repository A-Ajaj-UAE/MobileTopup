
namespace MobileTopup.Contracts.Models
{
    public class User
    {
        /// <example>1234567890 for verified user</example>
        public string PhoneNumber { get; set; }
        public  string Name { get; set; }
        public  string Remark { get; set; }
        public bool IsVerified { get; set; }
        public List<Beneficiary>? Beneficiaries { get; set; }
    }
}
