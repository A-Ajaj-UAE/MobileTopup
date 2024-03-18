using MobileTopup.Contracts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Response
{
    public class BeneficiaryResponse
    {
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
