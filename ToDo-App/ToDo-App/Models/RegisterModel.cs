using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }


        [Required(ErrorMessage = "LastName not specified")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "FirstName not specified")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "FirstName not specified")]
        public string Address { get; set; }
    }
}
