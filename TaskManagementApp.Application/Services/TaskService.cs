using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.Common.Exceptions;
using TaskManagementApp.Application.DTOs.Task;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Persistance;
using TaskManagementApp.Domain.Entities;
using TaskManagementApp.Domain.Enums;

namespace TaskManagementApp.Application.Services
{
    public class TaskService: ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskDetailDto> CreateTaskAsync(TaskCreateDto dto)
        {
            
            var project = await _context.Projects
                .Include(p => p.ProjectDevelopers)
                    .ThenInclude(pd => pd.Developer)
                .FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);

            if (project == null)
                throw new NotFoundException("Project not found.");

            if (dto.DueDate > project.Deadline)
                throw new System.ComponentModel.DataAnnotations.ValidationException("Task due date cannot exceed project deadline.");


            var developer = project.ProjectDevelopers
                .Select(pd => pd.Developer)
                .FirstOrDefault(d => d.UserId == dto.AssignedToId);

            if (developer == null)
                throw new System.ComponentModel.DataAnnotations.ValidationException("Assigned developer is not part of this project.");

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                AssignedToId = dto.AssignedToId,
                AssignedTo = developer,
                Status = TaskStatuss.Pending,
                DueDate = dto.DueDate
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDetailDto>(task);
        }


        public async Task<TaskDetailDto> UpdateTaskAsync(int id, TaskUpdateDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedTo) 
                .Include(t=>t.Project)
                .FirstOrDefaultAsync(t => t.TaskId == id);

            if(task.DueDate> task.Project.Deadline)
                throw new System.ComponentModel.DataAnnotations.ValidationException("Task due date cannot exceed project deadline.");

            if (task == null)
                throw new NotFoundException("Task not found.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDetailDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskDetailDto?> GetByIdAsync(int id)
        {
            var task = await _context.Tasks
                .Include(p => p.Project)
                .Include(u => u.AssignedTo)
                .FirstOrDefaultAsync(t => t.TaskId == id);

            return task == null ? null : _mapper.Map<TaskDetailDto>(task);
        }

        public async Task<bool> AssignTaskToDeveloperAsync(int taskId, int developerId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            var developer = await _context.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.UserId == developerId && u.Role.Name == "Developer");

            if (task == null || developer == null)
                throw new NotFoundException("Task or developer not found.");

            task.AssignedToId = developerId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, int userId, TaskStatusUpdateDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                throw new NotFoundException("Task not found.");

            
            if (task.AssignedToId != userId)
                throw new UnauthorizedException("Only the assigned developer can update this task's status.");

           
            task.Status = dto.Status;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
