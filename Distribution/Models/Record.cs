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
        public int Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "სახელი")]
        public string FirstName { get; set; }
        [Display(Name = "გვარი")]
        public string SecondName { get; set; }
        [Display(Name = "პირადი ნომერი")]
        public int PiradiNomeri { get; set; }
        public string PhoneNumber { get; set; }        
        public string ContractPictureName { get; set; }        
    }
}
