using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.Task.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.Services.Domain.Task
{
    public interface ITaskService
    {
        /// <summary>
        /// Creates a new task
        /// </summary>
        /// <param name="projectId">Project Id the task is under.</param>
        /// <param name="dto">The payload (TaskModel) to create a new task.</param>
        /// <see cref="TaskDto"/>
        /// <returns>Object of <see cref="TaskDto"/></returns>
        Task<ServiceResponse<TaskDto>> CreateTaskAsync(Guid? projectId, TaskDto model);

        /// <summary>
        /// Deletes task
        /// </summary>
        /// <param name="projectId">Project Id the task is under.</param>
        /// <param name="taskId">Id of the task to delete.</param>
        /// <returns>Object of <see cref="TaskDto"/></returns>
        Task<ServiceResponse<TaskDto>> DeleteTaskAsync(Guid projectId, Guid taskId);

        /// <summary>
        /// Get tasks by status 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <param name="requestParameters"></param>
        /// <param name="status">Enum representing the status of the task.</param>
        /// <see cref="Status"/>
        /// <returns> <see cref="PaginationResponse{T}" /></returns>
        Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByStatusAsync(Guid projectId, Status status, RequestParameters requestParameters);

        /// <summary>
        /// Get Tasks that it's due date has elapsed.
        /// </summary>
        /// <param name="taskId">Id of the task</param>
        /// <param name="requestParameters">Used to paginate items.</param>
        /// <param name="projectId">Project Id of the task.</param>
        /// <returns> <see cref="PaginationResponse{T}"/></returns>
        Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByDueDateForTheWeekAsync(Guid projectId, RequestParameters requestParameters);

        /// <summary>
        /// Assign a task to a project or remove it from a project.
        /// </summary>
        /// <param name="projectId">The projectId of the task</param>
        /// <param name="taskId">The Id of the task.</param>
        /// <param name="action">An enum to choose weather to assign or remove task from a project <see cref="TaskAction"/></param>
        /// <returns>Object of <see cref="TaskDto"/></returns>
        Task<ServiceResponse<TaskDto>> AssignOrRemoveTaskFromAProjectAsync(Guid projectId, Guid taskId, TaskAction action);
    }

}
