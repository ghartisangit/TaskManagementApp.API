using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Application.DTOs.Project
{
    public class ProjectUpdateDto
    {
      

        [Required, MaxLength(150)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }
    }
}
