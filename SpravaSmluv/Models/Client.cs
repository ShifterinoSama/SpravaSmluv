using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpravaSmluv.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PersonalIdentificationNumber), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Musíte vyplnit Jméno!"), RegularExpression(@"^[a-zA-Z\u0100-\u017F\u0180-\u024F\u1E00-\u1EFF\u00C0-\u00FF]+$", ErrorMessage = "Ujistěte se, že jméno začíná velkým písmenem a je ve správném formátu!")]
        [Display(Name = "Jméno")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Musíte vyplnit Přijmení!"), RegularExpression(@"^[a-zA-Z\u0100-\u017F\u0180-\u024F\u1E00-\u1EFF\u00C0-\u00FF]+$", ErrorMessage = "Ujistěte se, že přijmení začíná velkým písmenem a je ve správném formátu!")]
        [Display(Name = "Přijmení")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Musíte vyplnit Email!"), RegularExpression(@"[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*", ErrorMessage = "Zadejte email v platném formátu!")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Musíte vyplnit telefonní číslo!")]
        [RegularExpression(@"[\+]?[\0-9]+[\ ]?[0-9]+[\ ]?[0-9]+[\ ]?[0-9]+", ErrorMessage = "Vyplňte číslo v platném formátu! (Např:+420 123 456 789, 321 654 987, 789456123")]
        [Display(Name = "Telefonní číslo")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Musíte vyplnit rodné číslo!")]
        [Display(Name = "Rodné číslo")]
        public string PersonalIdentificationNumber { get; set; }

        [Required(ErrorMessage = "Musíte vyplnit věk!")]
        [Display(Name = "Věk")]
        [Range(18,100,ErrorMessage = "Zadejte prosím platný věk! (18-100)")]
        public int Age { get; set; }

        [Display(Name = "Klient")]
        public string FullName { get => FirstName + " " + LastName; private set { } }

        [Display(Name = "Evidenční číslo smluv")]
        public List<Contract> Contracts { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
