using System.Numerics;

namespace MobileTopup.Contracts.Models
{
    public class Beneficiary
    {
        public Beneficiary(){}
        public Beneficiary(string phone, bool isActive)
        {
            IsActive = isActive;
            NickName = phone;
            Phone = phone;
        }

        public Beneficiary(bool isActive, string nickName, string phone)
        {
            IsActive = isActive;
            NickName = nickName;
            Phone = phone;
        }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
