using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Domain.Entities
{
    public class TaskRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [Required]
        public int RequestedById { get; set; }  
        public User RequestedBy { get; set; } = null!;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public RequestStatuss Status { get; set; } = RequestStatuss.Pending;

    }
    public enum RequestStatuss
    {
        Pending = 1,
        Approved = 2,
        Rejected =3
    }
}
