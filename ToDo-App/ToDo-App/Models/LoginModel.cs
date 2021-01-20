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
        public string Email { get; set; }

        [Required(ErrorMessage = "PropertyPasswordRequiredError")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
