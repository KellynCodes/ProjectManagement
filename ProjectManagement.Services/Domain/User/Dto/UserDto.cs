using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.User.Dto;
public record UserDto([Required] string Name, [Required] string UserName) : BaseRecord;
public record UserRecordDto([Required] string Name, [Required] string UserName, IEnumerable<ProjectDto> Projects) : BaseRecord;
