using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Response
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
