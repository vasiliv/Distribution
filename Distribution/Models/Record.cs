using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Distribution.Models
{
    public class Record
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string SecondName { get; set; }
        [Display(Name = "Id Number")]
        public int PiradiNomeri { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Contract Photo")]
        public string ContractPictureName { get; set; }        
    }
}
