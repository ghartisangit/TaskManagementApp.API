using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementApp.Application.DTOs.Task;
using TaskManagementApp.Application.Interfaces;

namespace TaskManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskRequestController : ControllerBase
    {
        private readonly ITaskRequestService _service;

        public TaskRequestController(ITaskRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> CreateRequest([FromBody] TaskRequestCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var response = await _service.CreateRequestAsync(userId, dto);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _service.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpPut("{requestId}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            await _service.ApproveRequestAsync(requestId);
            return Ok("Task request approved and task created.");
        }

        [HttpPut("{requestId}/reject")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            await _service.RejectRequestAsync(requestId);
            return Ok("Task request rejected.");
        }
    }
}
