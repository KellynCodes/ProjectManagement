using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Controllers.Shared;
using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.Task;
using ProjectManagement.Services.Domain.Task.Dto;
using ProjectManagement.Services.Domain.User.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.API.Controllers;

[Route("api/v{version:apiVersion}/task")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
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
    [HttpPut("{projectId}/assign/{taskId}/{taskAction}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> AssignOrRemoveTaskFromAProject([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] TaskAction taskAction)
    {
        ServiceResponse<TaskDto> response = await _taskService.AssignOrRemoveTaskFromAProjectAsync(projectId, taskId, taskAction);
        return ComputeResponse(response);
    }


    [HttpPut("assign/{projectId}/{taskId}/{userId}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> AssignTaskToUser(Guid projectId, Guid taskId, string userId)
    {
        ServiceResponse<TaskDto> response = await _taskService.AssignTaskToUserAsync(projectId, taskId, userId, CancellationToken.None);
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

    [HttpPut("status/{taskId}/{userId}/{status}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<TaskDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> ChangeTaskStatus([FromRoute] Guid taskId, [FromRoute] string userId, [FromRoute] Status status)
    {
        ServiceResponse<TaskDto> response = await _taskService.ChangeTaskStatusAsync(taskId, userId, status, CancellationToken.None);
        return ComputeResponse(response);
    }

    [HttpGet("due-date-reminder")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<UserResponseDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> ChangeTaskStatus()
    {
        ServiceResponse<IEnumerable<UserResponseDto>> response = await _taskService.NotifyUserWhenTaskDueDateIsWithin48HrsAsync(CancellationToken.None);
        return ComputeResponse(response);
    }
}
