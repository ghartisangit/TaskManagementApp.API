using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Application.DTOs.Project;
using TaskManagementApp.Application.Interfaces;

namespace TaskManagementApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto dto)
        {
            
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("Invalid token: missing user ID.");

            if (!int.TryParse(userIdClaim.Value, out int managerId))
                return BadRequest("Invalid user ID in token.");

            
            var result = await _projectService.CreateProjectAsync(dto, managerId);
            if (result == null)
                return BadRequest("Failed to create project.");

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _projectService.GetAllAsync();
            if (result == null)
                return NotFound("No projects found.");
            return Ok(result);
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectUpdateDto dto)
        {
            var result = await _projectService.UpdateProjectAsync(id, dto);
            if (result == null)
                return NotFound("Project not found.");
            return Ok(result);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id, [FromQuery] bool forceDelete = false)
        {
            var result = await _projectService.DeleteProjectAsync(id, forceDelete);
            if(!result)
                return NotFound("Project not found or could not be deleted.");

            return Ok(new { Success = result, Message = "Project deleted successfully." });
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if(project == null)
                return NotFound("Project not found.");
            return Ok(project);
        }


        [HttpGet("{id}/progress")]
        public async Task<IActionResult> GetProjectProgress(int id)
        {
            var progress = await _projectService.GetProjectProgressAsync(id);
            if(progress == null)
                return NotFound("Project not found.");
            return Ok(progress);
        }

    }
}
