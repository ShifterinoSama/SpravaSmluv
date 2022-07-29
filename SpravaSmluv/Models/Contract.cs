using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpravaSmluv.Models
{
    public class Contract
    {
        public int ID { get; set; }
        [Required()]
        [Display(Name = "Evidenční číslo")]
        public string EvidenceNumber { get; set; }
        [Required()]
        [Display(Name = "Instituce")]
        public string Institution { get; set; }
        public Client Client { get; set; }
        public Advisor ContractManager { get; set; }
        [Required()]
        [Display(Name = "Datum uzavření smlouvy")]
        [DataType(DataType.Date)]
        public DateTime ClosureDate { get; set; }
        [Required()]
        [Display(Name = "Datum vypršení platnosti")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        [Display(Name = "Datum ukončení smlouvy")]
        [DataType(DataType.Date)]
        public DateTime TerminationDate { get; set; }
        public ICollection<ContractAdvisor> ContractAdvisors { get; set; }

        
    }
}
