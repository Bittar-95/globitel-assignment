using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASSIGNMENT.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name ="Gender")]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name ="Employee Statsu")]
        public Status EmStatus { get; set; }

        [Display(Name ="Birth Of Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime MyProperty { get; set; }

        [ForeignKey(name:"User")]
        public virtual User AdminID { get; set; }
        
    }
    public enum Gender { Male , Fmale}
    public enum Status { Active,Deactive}
}
