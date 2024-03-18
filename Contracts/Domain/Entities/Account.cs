using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<Transaction> Transactions { get; set; }


    }
}
