namespace MobileTopup.Contracts.Requests
{
    public class AddBeneficiaryRequest
    {
        public string PhoneNumber { get; set; }
        public string NickName { get; set; }
        public bool IsActive { get; set; }
    }
}
