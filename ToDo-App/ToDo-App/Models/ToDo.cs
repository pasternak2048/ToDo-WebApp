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

        [StringLength(maximumLength: 256, ErrorMessage = "The 'Task Name' field must not exceed 256 symbols")]
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
