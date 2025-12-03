using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.DTOs.Task
{
    public class TaskCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime? DueDate { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int AssignedToId { get; set; }
    }
}
