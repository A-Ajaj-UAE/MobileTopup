using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Requests
{
    public class TopupRequest
    {
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
