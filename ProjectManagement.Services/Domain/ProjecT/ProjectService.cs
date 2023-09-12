using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _projectRepo = _unitOfWork.GetRepository<Project>();
        }
        public async Task<ServiceResponse<ProjectDto>> CreateProjectAsync(string userId, CreateProjectDto model)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }


            Project project = new()
            {
                Name = model.Name,
                Description = model.Description,
                UserId = userId
            };
            project = await _projectRepo.AddAsync(project);
            await _userManager.UpdateAsync(user);
            ProjectDto result = _mapper.Map<ProjectDto>(project);

            return new ServiceResponse<ProjectDto>
            {
                Data = result,
                Message = $"{project.Name} has been created.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<ProjectDto>> DeleteProjectAsync(string userId, Guid projectId)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId && p.UserId == userId);

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

        public async Task<ServiceResponse<ProjectDto>> GetProjectAsync(string userId, Guid projectId)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId && p.UserId == userId);

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
                    Message = "Project is empty.",
                    StatusCode = HttpStatusCode.NoContent
                };
            }
            PaginationResponse<ProjectDto> result = _mapper.Map<PaginationResponse<ProjectDto>>(projects);

            return new ServiceResponse<PaginationResponse<ProjectDto>>
            {
                Data = result,
                Message = $"{projects.TotalRecords} projects found.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse<ProjectDto>> UpdateProjectAsync(string userId, Guid projectId, ProjectUpdateDto model)
        {
            Project project = await _projectRepo.GetSingleByAsync(p => p.Id == projectId && p.UserId == userId);

            if (project is null)
            {
                return new ServiceResponse<ProjectDto>
                {
                    Data = null,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Project not found.",
                };
            }

            _mapper.Map(model, project);
            await _projectRepo.UpdateAsync(project);

            return new ServiceResponse<ProjectDto>
            {
                Message = $"{project.Name} has been updated.",
                StatusCode = HttpStatusCode.OK
            };
        }

    }
}


