using AutoMapper;
using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Identity;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Domain.Task.Dto;
using ProjectManagement.Services.Domain.User.Dto;

namespace ProjectManagement.Services.Mapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User mapping
            CreateMap<UserDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();

            //Task mapping
            CreateMap<TaskDto, ProjTask>();
            CreateMap<ProjTask, TaskDto>();
            CreateMap<PaginationResult<ProjTask>, PaginationResponse<TaskDto>>();

            //project mapping
            CreateMap<ProjectDto, Project>();
            CreateMap<Project, ProjectDto>();
            CreateMap<Project, CreateProjectDto>();
            CreateMap<ProjectUpdateDto, Project>();
            CreateMap<CreateProjectDto, Project>();
            CreateMap<PaginationResult<Project>, PaginationResponse<ProjectDto>>();
        }
    }
}
