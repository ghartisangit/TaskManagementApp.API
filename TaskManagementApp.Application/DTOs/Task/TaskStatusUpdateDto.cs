using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Domain.Enums;

namespace TaskManagementApp.Application.DTOs.Task
{
    public class TaskStatusUpdateDto
    {
        [Required]
        public int TaskId { get; set; }

        [Required]
        public TaskStatuss Status { get; set; }
    }
}
