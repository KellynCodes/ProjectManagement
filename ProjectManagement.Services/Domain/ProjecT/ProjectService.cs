using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Utility;
using System.Net;

namespace ProjectManagement.Services.Domain.ProjecT
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Project> _projectRepo;
        private readonly IMapper _mapper;
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _projectRepo = _unitOfWork.GetRepository<Project>();
        }
        public async Task<ServiceResponse<ProjectDto>> CreateProjectAsync(ProjectDto model)
        {
            if (model is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "Payload cannot be empty.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            Project project = _mapper.Map<Project>(model);
            project = await _projectRepo.AddAsync(project);
            ProjectDto result = _mapper.Map<ProjectDto>(project);

            return new ServiceResponse<ProjectDto>
            {
                Data = result,
                Message = $"{project.Name} has been created.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<ProjectDto>> DeleteProjectAsync(Guid projectId)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Project not found.",
                };
            }

            await _projectRepo.DeleteAsync(project);

            return new ServiceResponse<ProjectDto>
            {
                Message = $"{project.Name} has been deleted.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<ProjectDto>> GetProjectAsync(Guid projectId)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Project not found.",
                };
            }
            ProjectDto result = _mapper.Map<ProjectDto>(project);
            return new ServiceResponse<ProjectDto>
            {
                Message = $"{project.Name}",
                StatusCode = HttpStatusCode.OK,
                Data = result
            };
        }

        public async Task<ServiceResponse<PaginationResponse<ProjectDto>>> GetProjectsAsync(RequestParameters requestParameters)
        {
            PaginationResult<Project> projects = await _projectRepo.GetPagedItems(
                 requestParameters,
                  predicate: x => x.IsActive,
                  orderBy: o => o.OrderBy(x => x.Name.Contains(requestParameters.Keywords)),
                  include: inc => inc.Include(x => x.Tasks)
                 );


            if (projects.TotalRecords <= 0)
            {
                return new ServiceResponse<PaginationResponse<ProjectDto>>
                {
                    Data = null,
                    Message = "Task is empty.",
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            PaginationResponse<ProjectDto> result = _mapper.Map<PaginationResponse<ProjectDto>>(projects);

            return new ServiceResponse<PaginationResponse<ProjectDto>>
            {
                Data = result,
                Message = $"{projects.TotalRecords} tasks found.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<ProjectDto>> UpdateProjectAsync(Guid projectId, ProjectDto model)
        {
            Project project = await _projectRepo.GetByIdAsync(projectId);
            if (project is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Project not found.",
                };
            }

            await _projectRepo.UpdateAsync(project);
            return new ServiceResponse<ProjectDto>
            {
                Message = $"{project.Name} has been deleted.",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}


