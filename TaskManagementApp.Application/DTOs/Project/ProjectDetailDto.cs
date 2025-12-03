using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.DTOs.Task;

namespace TaskManagementApp.Application.DTOs.Project
{
    public class ProjectDetailDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public double Progress { get; set; }
        public List<string>? AssignedDevelopers { get; set; }
        public List<TaskDetailDto>? Tasks { get; set; }
    }
}
