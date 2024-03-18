using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileTopup.Contracts.Domain.Entities
{
    public class TopupOption
    {
        public TopupOption()
        {
        }

        public TopupOption(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
