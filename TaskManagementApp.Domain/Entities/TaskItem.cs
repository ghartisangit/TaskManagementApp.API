using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Domain.Enums;

namespace TaskManagementApp.Domain.Entities
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public TaskStatuss Status { get; set; } = TaskStatuss.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

       
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        public Project? Project { get; set; }

       
        [ForeignKey(nameof(AssignedTo))]
        public int AssignedToId { get; set; }

        public User? AssignedTo { get; set; }
    }
}
