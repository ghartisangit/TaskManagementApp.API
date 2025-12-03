using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.DTOs.Task;


namespace TaskManagementApp.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDetailDto> CreateTaskAsync(TaskCreateDto dto);
        Task<TaskDetailDto> UpdateTaskAsync(int id, TaskUpdateDto dto);
        Task<bool> DeleteTaskAsync(int id);
        Task<TaskDetailDto?> GetByIdAsync(int id);
        Task<bool> AssignTaskToDeveloperAsync(int taskId, int developerId);
        Task<bool> UpdateTaskStatusAsync(int taskId, int userId, TaskStatusUpdateDto dto);

    
    }
}
