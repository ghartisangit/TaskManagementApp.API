using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.Common.Exceptions;
using TaskManagementApp.Application.DTOs.Task;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Persistance;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Services
{
    public class TaskRequestService: ITaskRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskRequestService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskRequestResponseDto> CreateRequestAsync(int developerId, TaskRequestCreateDto dto)
        {
          
            var project = await _context.Projects
                .Include(p => p.ProjectDevelopers) 
                .ThenInclude(pd => pd.Developer)
                .FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);

            if (project == null)
                throw new NotFoundException("Project not found.");

           
            var isAssigned = project.ProjectDevelopers.Any(pd => pd.DeveloperId == developerId);
            if (!isAssigned)
                throw new UnauthorizedAccessException("You are not assigned to this project, so you cannot make a task request.");

           
            var request = new TaskRequest
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                RequestedById = developerId,
                Deadline= dto.Deadline
            };

            _context.TaskRequests.Add(request);
            await _context.SaveChangesAsync();

         
            var loadedRequest = await _context.TaskRequests
                .Include(r => r.RequestedBy)
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.RequestId == request.RequestId);

            return _mapper.Map<TaskRequestResponseDto>(loadedRequest);
        }


        public async Task<IEnumerable<TaskRequestResponseDto>> GetAllRequestsAsync()
        {
            var requests = await _context.TaskRequests
                .Include(r => r.RequestedBy)
                .Include(r => r.Project)
                    .ThenInclude(p => p.Tasks)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskRequestResponseDto>>(requests);
        }

        public async Task<bool> ApproveRequestAsync(int requestId)
        {
            var request = await _context.TaskRequests
                .Include(r => r.Project)
                .Include(r => r.RequestedBy)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);

            if (request == null)
                throw new NotFoundException("Task request not found.");

            request.Status = RequestStatuss.Approved;
           
            var newTask = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectId,
                AssignedToId = request.RequestedById,
                Status = Domain.Enums.TaskStatuss.Pending,
                DueDate= request.Deadline,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectRequestAsync(int requestId)
        {
            var request = await _context.TaskRequests.FindAsync(requestId);
            if (request == null)
                throw new NotFoundException("Task request not found.");

            request.Status = RequestStatuss.Rejected;
            

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
