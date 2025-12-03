using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Domain.Entities
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime Deadline { get; set; }  
        
        [ForeignKey(nameof(Manager))]
        public int ManagerId { get; set; } 
        public User Manager { get; set; } = null;

        public ICollection<TaskItem>? Tasks { get; set; }

        public ICollection<ProjectDeveloper> ProjectDevelopers { get; set; } = new List<ProjectDeveloper>();
    }

    public class ProjectDeveloper
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int DeveloperId { get; set; }
        public User Developer { get; set; } = null!;
    }
}
