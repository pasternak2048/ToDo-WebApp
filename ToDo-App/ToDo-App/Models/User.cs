using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "PropertyEmailRequiredError")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "IncorrectEmailError")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PropertyPasswordRequiredError")]
        [StringLength(maximumLength: 16, ErrorMessage = "PropertyPasswordLengthError")]
        public string Password { get; set; }

        [Required(ErrorMessage = "PropertyLastNameRequiredError")]
        [StringLength(maximumLength: 50, ErrorMessage = "PropertyLastNameLengthError")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "PropertyFirstNameRequiredError")]
        [StringLength(maximumLength: 50, ErrorMessage = "PropertyFirstNameLengthError")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "PropertyAddressRequiredError")]
        [StringLength(maximumLength: 50, ErrorMessage = "PropertyAddressLengthError")]
        public string Address { get; set; }
        public DateTimeOffset DateOfRegistration { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public List<ToDo> ToDos;
        public User()
        {
            ToDos = new List<ToDo>();
        }
    }
}
