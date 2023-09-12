using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Controllers.Shared;
using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.Task;
using ProjectManagement.Services.Domain.Task.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.API.Controllers;

[Route("api/v{version:apiVersion}/task")]
[ApiVersion("1.0")]
[ApiController]
public class TaskController : BaseController
{
    private readonly ITaskService _taskService;
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Create new Task
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> CreateTask([FromBody] TaskDto model, Guid? projectId = null)
    {
        ServiceResponse<TaskDto> response = await _taskService.CreateTaskAsync(projectId, model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Assign or Remove task from a project
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="taskId"></param>
    /// <param name="taskAction"></param>
    /// <returns></returns>
    [HttpPut("assign/{projectId}/{taskId}/{taskAction}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> AssignOrRemoveTaskFromAProject([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] TaskAction taskAction)
    {
        ServiceResponse<TaskDto> response = await _taskService.AssignOrRemoveTaskFromAProjectAsync(projectId, taskId, taskAction);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Gets all tasks that are due for the week
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="requestParameters"></param>
    /// <returns></returns>
    [HttpGet("due/{projectId}")]
    [ProducesResponseType(200, Type = typeof(ApiRecordResponse<PaginationResponse<TaskDto>>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetTasksByDueDateForTheWeek([FromRoute] Guid projectId, [FromQuery] RequestParameters requestParameters)
    {
        ServiceResponse<PaginationResponse<TaskDto>> response = await _taskService.GetTasksByDueDateForTheWeekAsync(projectId, requestParameters);
        return ComputeResponse(response);
    }

    [HttpGet("status/{projectId}/{status}")]
    [ProducesResponseType(200, Type = typeof(ApiRecordResponse<PaginationResponse<TaskDto>>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetDueTasksByStatus([FromRoute] Guid projectId, [FromRoute] Status status, [FromQuery] RequestParameters requestParameters)
    {
        ServiceResponse<PaginationResponse<TaskDto>> response = await _taskService.GetTasksByStatusAsync(projectId, status, requestParameters);
        return ComputeResponse(response);
    }

    [HttpPut("update/{projectId}/{taskId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] TaskDto model)
    {
        ServiceResponse<TaskDto> response = await _taskService.UpdateTaskAsync(projectId, taskId, model);
        return ComputeResponse(response);
    }
}