using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.Task.Dto;
using ProjectManagement.Services.Utility;
using System.Net;

namespace ProjectManagement.Services.Domain.Task
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<Project> _projectRepo;
        private readonly IRepository<ProjTask> _taskRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        public TaskService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TaskService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _taskRepo = _unitOfWork.GetRepository<ProjTask>();
            _projectRepo = _unitOfWork.GetRepository<Project>();
        }
        public async Task<ServiceResponse<TaskDto>> AssignOrRemoveTaskFromAProjectAsync(Guid projectId, Guid taskId, TaskAction action)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            ProjTask task = await _taskRepo.GetByIdAsync(taskId);
            if (task is null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Task not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            switch (action)
            {
                case TaskAction.Assign:
                    task.ProjectId = projectId;
                    break;
                case TaskAction.Remove:
                    task.ProjectId = null;
                    break;
                default:
                    break;
            }
            task = await _taskRepo.UpdateAsync(task);
            TaskDto result = _mapper.Map<TaskDto>(task);
            string taskAction = action.ToString();
            return new ServiceResponse<TaskDto>
            {
                Data = result,
                Message = $"{task.Title} has been {taskAction}ed to {project.Name}",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<TaskDto>> CreateTaskAsync(Guid? projectId, TaskDto model)
        {
            Project project = null!;
            if (projectId is not null)
            {
                project = await _projectRepo.GetByIdAsync(projectId);
            }

            ProjTask task = _mapper.Map<ProjTask>(model);
            task.ProjectId = project?.Id;
            _taskRepo.Add(task);
            int numberOfAffectedRows = await _taskRepo.SaveAsync();
            if (numberOfAffectedRows <= 0)
            {
                _logger.LogError($"Something unexpected happened while creating task with projectId of {projectId} on {DateTimeOffset.UtcNow}");
                throw new DbUpdateException($"Something unexpected happened while creating task with projectId of {projectId} on {DateTimeOffset.UtcNow}");
            }

            string message = project is not null ? $"{task.Title} has been created under {project.Name}" : "Task has been created. You can assign it to a project later.";
            return new ServiceResponse<TaskDto>
            {
                Data = model,
                StatusCode = HttpStatusCode.OK,
                Message = message
            };

        }

        public async Task<ServiceResponse<TaskDto>> DeleteTaskAsync(Guid projectId, Guid taskId)
        {

            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            ProjTask task = await _taskRepo.GetSingleByAsync(x => x.ProjectId == projectId && x.Id == taskId);
            if (task is null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            bool IsDeleted = _taskRepo.Delete(task);
            int numberOfAffectedRows = await _taskRepo.SaveAsync();
            if (!IsDeleted || numberOfAffectedRows <= 0)
            {
                _logger.LogError($"Something unexpected happened while creating task with projectId of {projectId} on {DateTimeOffset.Now}");
                throw new DbUpdateException($"Something unexpected happened while creating task with projectId of {projectId} on {DateTimeOffset.Now}");
            }

            return new ServiceResponse<TaskDto>
            {
                Data = null,
                StatusCode = HttpStatusCode.OK,
                Message = "Task has been created."
            };
        }

        public async Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByDueDateForTheWeekAsync(Guid projectId, RequestParameters requestParameters)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<PaginationResponse<TaskDto>>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            PaginationResult<ProjTask> tasks = await _taskRepo.GetPagedItems(
                requestParameters,
                 predicate: t => t.Status == Status.InProgress && t.DueDate.AddDays(7) <= DateTime.UtcNow.AddDays(7),
                 orderBy: o => o.OrderBy(t => t.Priority == Priority.High),
                 include: inc => inc.Include(t => t.Project)
                 );


            if (tasks.TotalRecords <= 0)
            {
                return new ServiceResponse<PaginationResponse<TaskDto>>
                {
                    Data = null,
                    Message = "Task is empty.",
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            PaginationResponse<TaskDto> result = _mapper.Map<PaginationResponse<TaskDto>>(tasks);

            return new ServiceResponse<PaginationResponse<TaskDto>>
            {
                Data = result,
                Message = $"{tasks.TotalRecords} tasks found.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<PaginationResponse<TaskDto>>> GetTasksByStatusAsync(Guid projectId, Status status, RequestParameters requestParameters)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<PaginationResponse<TaskDto>>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            PaginationResult<ProjTask> tasks = await _taskRepo.GetPagedItems(
                requestParameters,
                 predicate: t => t.Status == status && t.IsActive,
                 orderBy: o => o.OrderBy(t => t.Priority == Priority.High),
                 include: inc => inc.Include(t => t.Project)
                 );


            if (tasks.TotalRecords <= 0)
            {
                return new ServiceResponse<PaginationResponse<TaskDto>>
                {
                    Data = null,
                    Message = "Task is empty.",
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            PaginationResponse<TaskDto> result = _mapper.Map<PaginationResponse<TaskDto>>(tasks);

            return new ServiceResponse<PaginationResponse<TaskDto>>
            {
                Data = result,
                Message = $"{tasks.TotalRecords} tasks found."
            };
        }

        public async Task<ServiceResponse<TaskDto>> UpdateTaskAsync(Guid projectId, Guid taskId, TaskDto model)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Project not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            ProjTask task = await _taskRepo.GetSingleByAsync(x => x.ProjectId == projectId && x.Id == taskId);
            if (task is null)
            {
                return new ServiceResponse<TaskDto>
                {
                    Message = "Task not found.",
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null
                };
            }

            _mapper.Map(model, task);
            task = await _taskRepo.UpdateAsync(task);
            TaskDto result = _mapper.Map<TaskDto>(task);
            return new ServiceResponse<TaskDto>
            {
                Data = result,
                Message = $"{model.Title} has been updated.",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
