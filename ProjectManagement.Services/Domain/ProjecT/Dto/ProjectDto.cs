using ProjectManagement.Models.Utility;

namespace ProjectManagement.Services.Domain.ProjecT.Dto;
public record ProjectDto(Guid Id, string Name, string Description, DateTime CreatedAt) : BaseRecord;


