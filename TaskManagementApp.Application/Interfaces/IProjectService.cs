using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.DTOs.Project;

namespace TaskManagementApp.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDetailDto> CreateProjectAsync(ProjectCreateDto dto, int managerId);
        Task<ProjectDetailDto> UpdateProjectAsync(int id, ProjectUpdateDto dto);
        Task<bool> DeleteProjectAsync(int id, bool forceDelete = false);
        Task<ProjectDetailDto> GetProjectByIdAsync(int projectId);
        Task<IEnumerable<ProjectDetailDto>> GetAllAsync();
        Task<ProjectProgressDto> GetProjectProgressAsync(int projectId);
    }
}
