using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "PropertyEmailRequiredError")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PropertyPasswordRequiredError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "PropertyPasswordComparedRequiredError")]
        [Compare("Password", ErrorMessage = "PropertyPasswordComparedError")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }


        [Required(ErrorMessage = "PropertyLastNameRequiredError")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "PropertyFirstNameRequiredError")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "PropertyAddressNameRequiredError")]
        public string Address { get; set; }
    }
}
