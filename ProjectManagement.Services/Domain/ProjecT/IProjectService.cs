using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.Services.Domain.ProjecT
{
    public interface IProjectService
    {
        Task<ServiceResponse<ProjectDto>> CreateProjectAsync(ProjectDto model);
        Task<ServiceResponse<ProjectDto>> DeleteProjectAsync(Guid projectId);
        Task<ServiceResponse<ProjectDto>> UpdateProjectAsync(Guid projectId, ProjectDto model);
        Task<ServiceResponse<ProjectDto>> GetProjectAsync(Guid projectId);
        Task<ServiceResponse<PaginationResponse<ProjectDto>>> GetProjectsAsync(RequestParameters requestParameters);
    }
}


