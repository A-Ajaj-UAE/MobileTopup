using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Requests
{
    public class CreateUserRequest
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool IsVerified { get; set; }
    }
}
