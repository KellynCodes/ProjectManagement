using ProjectManagement.Models.Entities.Domains.Project;
using ProjectManagement.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.User.Dto;
public record UserDto([Required] string Name, [Required] string UserName) : BaseRecord;
public record UserRecordDto(string Name, string UserName, IEnumerable<Project> Projects) : BaseRecord;
