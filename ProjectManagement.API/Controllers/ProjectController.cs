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
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> CreateTask([FromBody] ProjectDto model)
    {
        ServiceResponse<ProjectDto> response = await _projectService.CreateProjectAsync(model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Update project
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("update/{projectId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid projectId, [FromBody] ProjectDto model)
    {
        ServiceResponse<ProjectDto> response = await _projectService.UpdateProjectAsync(projectId, model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Get all project
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="requestParameters"></param>
    /// <returns></returns>
    [HttpGet("get-all")]
    [ProducesResponseType(200, Type = typeof(ApiRecordResponse<PaginationResponse<ProjectDto>>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetTasksByDueDateForTheWeek([FromQuery] RequestParameters requestParameters)
    {
        ServiceResponse<PaginationResponse<ProjectDto>> response = await _projectService.GetProjectsAsync(requestParameters);
        return ComputeResponse(response);
    }

    /// <summary>
    /// get single project
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("get/{projectId}")]
    [ProducesResponseType(200, Type = typeof(ApiRecordResponse<ProjectDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetDueTasksByStatus([FromRoute] Guid projectId)
    {
        ServiceResponse<ProjectDto> response = await _projectService.GetProjectAsync(projectId);
        return ComputeResponse(response);
    }
}