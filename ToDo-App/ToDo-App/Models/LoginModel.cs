using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "PropertyEmailRequiredError")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "IncorrectEmailError")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PropertyPasswordRequiredError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
