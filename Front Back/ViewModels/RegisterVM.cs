using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Front_Back.ViewModels
{
    public class RegisterVM
    {
        public string Fullname { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required,StringLength(maximumLength:20)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare(nameof(Password),ErrorMessage ="Confirm Password and Password do n ot watch")]
        public  string ConfirmPassword { get; set; }

    }
}
