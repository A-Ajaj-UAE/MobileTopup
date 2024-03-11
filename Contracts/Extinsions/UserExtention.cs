using MobileTopup.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Extensions
{
    public static class UserExtention
    {
        public static User AddBeneficiary(this User user, Beneficiary beneficiary)
        {
            if (user.Beneficiaries == null)
            {
                user.Beneficiaries = new List<Beneficiary>();
            }

            user.Beneficiaries.Add(beneficiary);

            return user;
        }

    }
}
