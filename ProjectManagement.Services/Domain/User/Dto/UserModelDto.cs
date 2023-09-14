using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Domain.Task.Dto;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.User.Dto;
public record UserModelDto([Required] string Name, [Required] string UserName) : BaseRecord;
public record UserRecordDto([Required] string Name, [Required] string UserName, IEnumerable<ProjectDto> Projects) : BaseRecord;

public record UserResponseDto : BaseRecord
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public IEnumerable<TaskRecordResponseDto> Task { get; set; }
}