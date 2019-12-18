using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASSIGNMENT.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }


        [Display(Name ="User Name")]
        [Required]
        public string UserName { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }


        [Display(Name = "Confirm-Pass")]
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password" , ErrorMessage = "The Password Does Not Match")]
        public string ConfirmPassword { get; set; }
    }
}
