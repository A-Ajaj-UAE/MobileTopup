using System.Numerics;

namespace MobileTopup.Contracts.Domain.Entities
{
    public class Beneficiary
    {
        public Beneficiary(){}
        public Beneficiary(string phone, bool isActive)
        {
            IsActive = isActive;
            NickName = phone;
            PhoneNumber = phone;
        }

        public Beneficiary(bool isActive, string nickName, string phone)
        {
            IsActive = isActive;
            NickName = nickName;
            PhoneNumber = phone;
        }
        public int Id { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
        public string UserPhoneNumber { get; set; }
    }
}
