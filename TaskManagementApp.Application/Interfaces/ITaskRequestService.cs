using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Application.DTOs.Task;

namespace TaskManagementApp.Application.Interfaces
{
    public interface ITaskRequestService
    {
        Task<TaskRequestResponseDto> CreateRequestAsync(int developerId, TaskRequestCreateDto dto);
        Task<IEnumerable<TaskRequestResponseDto>> GetAllRequestsAsync();
        Task<bool> ApproveRequestAsync(int requestId);
        Task<bool> RejectRequestAsync(int requestId);
    }
}
