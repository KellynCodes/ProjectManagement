using AutoMapper;
using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Domain.Task.Dto;

namespace ProjectManagement.Services.Mapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskDto, ProjTask>();
            CreateMap<ProjTask, TaskDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<Project, ProjectDto>();
            CreateMap<PaginationResult<TaskDto>, PaginationResponse<TaskDto>>();
        }
    }
}
