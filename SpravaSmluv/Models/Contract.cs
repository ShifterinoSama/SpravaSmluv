using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpravaSmluv.Models
{
    
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Index(nameof(EvidenceNumber), IsUnique = true)]
        [Display(Name = "Evidenční číslo")]
        public string EvidenceNumber { get; set; }

        [Required(ErrorMessage = "Musíte zadat instituci!")]
        [Display(Name = "Instituce")]
        public string Institution { get; set; }

        [Required(ErrorMessage = "Musíte vybrat klienta!")]
        public int ClientId { get; set; }

        [Display(Name = "Klient")]
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required(ErrorMessage = "Musíte vybrat správce smlouvy!!")]
        public int ContractManagerId { get; set; }

        [ForeignKey("ContractManagerId")]
        [Display(Name = "Správce smlouvy")]
        public Advisor ContractManager { get; set; }

        [Required(ErrorMessage = "Musíte zadat datum uzavření smlouvy!")]
        [Display(Name = "Datum uzavření smlouvy")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ClosureDate { get; set; }

        [Required(ErrorMessage = "Musíte zadat datum vypršení platnosti smlouvy!")]
        [Display(Name = "Datum vypršení platnosti")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Datum ukončení smlouvy")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        [Display(Name = "Poradci")]
        public virtual ICollection<Advisor> Advisors { get; set; }
       
    }
}
