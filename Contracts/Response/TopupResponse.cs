using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Response
{
    public class TopupResponse
    {
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
