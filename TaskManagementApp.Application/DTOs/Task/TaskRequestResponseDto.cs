using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.DTOs.Task
{
    public class TaskRequestResponseDto
    {
        public int RequestId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public string ProjectTitle { get; set; } = string.Empty;
        public RequestStatusDto Status { get; set; } = new();

        

    }
    public class RequestStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class TaskSummaryDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
