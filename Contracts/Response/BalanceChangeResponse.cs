namespace MobileTopup.Contracts.Response
{
    public class BalanceChangeResponse
    {
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
    }
}
