using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "PropertyNameRequiredError")]
        [StringLength(maximumLength: 256, ErrorMessage = "PropertyNameLengthError")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "PropertyDeadlineError")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
