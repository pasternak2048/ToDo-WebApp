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
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "IncorrectEmailError")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PropertyPasswordRequiredError")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "IncorrectPasswordError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "PropertyPasswordComparedRequiredError")]
        [Compare("Password", ErrorMessage = "PropertyPasswordComparedError")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }


        [Required(ErrorMessage = "PropertyLastNameRequiredError")]
        [RegularExpression(@"[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~< >;:[\]]{2,}", ErrorMessage = "IncorrectLastNameError")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "PropertyFirstNameRequiredError")]
        [RegularExpression(@"[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~< >;:[\]]{2,}", ErrorMessage = "IncorrectFirstNameError")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "PropertyAddressNameRequiredError")]
        public string Address { get; set; }
    }
}
