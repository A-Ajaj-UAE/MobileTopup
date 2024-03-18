using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Domain.Entities
{
    public class Transaction
    {
        public Transaction()
        {
        }

        public Transaction(string phoneNumber, decimal amount, DateTime date)
        {
            PhoneNumber = phoneNumber;
            Amount = amount;
            Date = date;
        }
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
    }
}
