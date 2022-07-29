using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpravaSmluv.Models
{
    public class Client
    {
        public int ID { get; set; }
        [Required()]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; }
        [Required()]
        [Display(Name = "Přijmení")]
        public string LastName { get; set; }
        [Required()]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required()]
        [Display(Name = "Telefonní číslo")]
        public string PhoneNumber { get; set; }
        [Required()]
        [Display(Name = "Rodné číslo")]
        public string PersonalIdentificationNumber { get; set; }
        [Required()]
        [Display(Name = "Věk")]
        public int Age { get; set; }
    }
}
