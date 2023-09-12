using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Controllers.Shared;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.API.Controllers;

[Route("api/v{version:apiVersion}/project")]
[ApiVersion("1.0")]
[ApiController]
public class ProjectController : BaseController
{
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Create new project
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("{userId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> CreateTask([FromRoute] string userId, [FromBody] CreateProjectDto model)
    {
        ServiceResponse<ProjectDto> response = await _projectService.CreateProjectAsync(userId, model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Update project
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="userId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("update/{userId}/{projectId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> UpdateProject([FromRoute] string userId, [FromRoute] Guid projectId, [FromBody] ProjectUpdateDto model)
    {
        ServiceResponse<ProjectDto> response = await _projectService.UpdateProjectAsync(userId, projectId, model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Get all project
    /// </summary>
    /// <param name="requestParameters"></param>
    /// <returns></returns>
    [HttpGet("get-all")]
    [ProducesResponseType(200, Type = typeof(ApiRecordResponse<PaginationResponse<ProjectDto>>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetProjects([FromQuery] RequestParameters requestParameters)
    {
        ServiceResponse<PaginationResponse<ProjectDto>> response = await _projectService.GetProjectsAsync(requestParameters);
        return ComputeResponse(response);
    }

    /// <summary>
    /// get single project
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("get/{userId}/{projectId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetProject([FromRoute] string userId, [FromRoute] Guid projectId)
    {
        ServiceResponse<ProjectDto> response = await _projectService.GetProjectAsync(userId, projectId);
        return ComputeResponse(response);
    }

    [HttpDelete("delete/{userId}/{projectId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> DeleteProject([FromRoute] string userId, [FromRoute] Guid projectId)
    {
        ServiceResponse<ProjectDto> response = await _projectService.DeleteProjectAsync(userId, projectId);
        return ComputeResponse(response);
    }
}