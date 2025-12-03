using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.Common.Exceptions;
using TaskManagementApp.Application.DTOs.Task;
using TaskManagementApp.Application.Interfaces;

namespace TaskManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

       
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            try
            {
                var result = await _taskService.CreateTaskAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

       

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var result = await _taskService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Task not found." });
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] TaskStatusUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            await _taskService.UpdateTaskStatusAsync(taskId, userId, dto);

            return Ok("Task status updated successfully.");
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
        {
            var result = await _taskService.UpdateTaskAsync(id, dto);
            if (result == null)
                return NotFound(new { message = "Task not found." });
            return Ok(result);
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted)
                return NotFound(new { message = "Task not found." });

            return Ok(new { message = "Task deleted successfully." });
        }
    }
}
