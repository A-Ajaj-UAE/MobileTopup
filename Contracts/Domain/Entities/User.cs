
namespace MobileTopup.Contracts.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public  string Name { get; set; }
        public  string Remark { get; set; }
        public bool IsVerified { get; set; }
        public List<Beneficiary>? Beneficiaries { get; set; }
        public Account Account { get; set; }
        public List<TopupHistory> TopupHistories { get; set; }
    }
}
