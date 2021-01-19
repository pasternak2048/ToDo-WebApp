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

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Incorrect Email")]
        public string Email { get; set; }
        public string Password { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "The 'Last Name' field must not exceed 50 symbols")]
        public string LastName { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "The 'First Name' field must not exceed 50 symbols")]
        public string FirstName { get; set; }
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
