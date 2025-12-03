using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.Common.Exceptions;
using TaskManagementApp.Application.DTOs.Project;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Persistance;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Services
{
    public class ProjectService: IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDetailDto> CreateProjectAsync(ProjectCreateDto dto, int managerId)
        {
            var manager = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.UserId == managerId && u.Role.Name == "Manager");

            if (manager == null)
                throw new UnauthorizedException("Only Managers can create projects.");

          
            var developers = await _context.Users
                .Where(u => dto.DeveloperIds.Contains(u.UserId) && u.Role.Name == "Developer")
                .ToListAsync();

            if (developers.Count != dto.DeveloperIds.Count)
                throw new ValidationException("One or more developer IDs are invalid.");

            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                Deadline = dto.Deadline,
                ManagerId = managerId,
            };

            
            foreach (var dev in developers)
            {
                project.ProjectDevelopers.Add(new ProjectDeveloper
                {
                    DeveloperId = dev.UserId,
                    Project = project
                });
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var projectWithDetails = await _context.Projects
               .Include(p => p.Manager)
               .Include(p => p.Tasks)
                   .ThenInclude(t => t.AssignedTo)
               .Include(p => p.ProjectDevelopers)
                   .ThenInclude(pd => pd.Developer)
               .FirstOrDefaultAsync(p => p.ProjectId == project.ProjectId);

            return _mapper.Map<ProjectDetailDto>(projectWithDetails!);
        }

        public async Task<ProjectDetailDto> UpdateProjectAsync(int id, ProjectUpdateDto dto)
        {
            var project = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedTo)
                .Include(p => p.ProjectDevelopers)
                    .ThenInclude(pd => pd.Developer)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
                throw new NotFoundException("Project not found.");

            
            project.Title = dto.Title;
            project.Description = dto.Description;
            project.Deadline = dto.Deadline;

            await _context.SaveChangesAsync();

            var updatedProject = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedTo)
                .Include(p => p.ProjectDevelopers)
                    .ThenInclude(pd => pd.Developer)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            return _mapper.Map<ProjectDetailDto>(updatedProject);
        }


        public async Task<bool> DeleteProjectAsync(int id, bool forceDelete = false)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
                throw new NotFoundException("Project not found.");

            bool hasTasks = project.Tasks != null && project.Tasks.Any();

            bool hasIncompleteTasks = hasTasks && project.Tasks.Any(t => t.Status != Domain.Enums.TaskStatuss.Completed);
           
            if (!hasTasks || !hasIncompleteTasks)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }

            
            if (hasIncompleteTasks && !forceDelete)
                throw new ValidationException("Cannot delete project because some tasks are not completed.");

            
            if (forceDelete)
            {
                _context.Tasks.RemoveRange(project.Tasks);
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<ProjectDetailDto> GetProjectByIdAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedTo)
                .Include(p => p.ProjectDevelopers)
                    .ThenInclude(pd => pd.Developer)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            return _mapper.Map<ProjectDetailDto>(project);
        }

       
        public async Task<IEnumerable<ProjectDetailDto>> GetAllAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedTo)
                .Include(p => p.ProjectDevelopers)
                    .ThenInclude(pd => pd.Developer)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectDetailDto>>(projects);
        }


        public async Task<ProjectProgressDto> GetProjectProgressAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks) 
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
                throw new NotFoundException("Project not found.");

            var totalTasks = project.Tasks.Count;
            var completedTasks = project.Tasks.Count(t => t.Status == Domain.Enums.TaskStatuss.Completed);
            var progress = totalTasks == 0 ? 0 : (completedTasks * 100.0) / totalTasks;

            return new ProjectProgressDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.Title,
                TotalTasks = totalTasks,       
                CompletedTasks = completedTasks, 
                ProgressPercent = progress
            };
        }

    }
}
