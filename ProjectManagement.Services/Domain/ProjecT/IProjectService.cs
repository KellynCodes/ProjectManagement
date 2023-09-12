using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.Services.Domain.ProjecT
{
    public interface IProjectService
    {
        Task<ServiceResponse<ProjectDto>> CreateProjectAsync(string userId, CreateProjectDto model);
        Task<ServiceResponse<ProjectDto>> DeleteProjectAsync(string userId, Guid projectId);
        Task<ServiceResponse<ProjectDto>> UpdateProjectAsync(string userId, Guid projectId, ProjectUpdateDto model);
        Task<ServiceResponse<ProjectDto>> GetProjectAsync(string userId, Guid projectId);
        Task<ServiceResponse<PaginationResponse<ProjectDto>>> GetProjectsAsync(RequestParameters requestParameters);
    }
}


