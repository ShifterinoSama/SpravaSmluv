using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpravaSmluv.Models
{
    public class ContractAdvisor
    {
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int AdvisorId { get; set; }
        public Advisor Advisor { get; set; }

    }
}
