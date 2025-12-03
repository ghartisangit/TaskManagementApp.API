using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.DTOs.Task
{
    public class TaskDetailDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatussDto Status { get; set; } = new TaskStatussDto();

        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignedTo { get; set; } = string.Empty;
    }
}
