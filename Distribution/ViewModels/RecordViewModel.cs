using Distribution.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Distribution.Areas.User.ViewModels
{
    public class RecordViewModel : EditImageViewModel
    {        
        [Display(Name = "სახელი")]
        public string FirstName { get; set; }
        [Display(Name = "გვარი")]
        public string SecondName { get; set; }
        [Display(Name = "პირადი ნომერი")]
        public int PiradiNomeri { get; set; }
        [Display(Name = "ტელეფონი")]
        public string PhoneNumber { get; set; }        
        //public IFormFile ContractPicture { get; set; }
        //public string ContractPictureName { get; set; }
        //public string ExistingContractPicture { get; set; }
    }
}
