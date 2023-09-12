using ProjectManagement.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.ProjecT.Dto;
public record ProjectDto([Required] Guid Id, [Required] string UserId, [Required] string Name, [Required] string Description, DateTime CreatedAt) : BaseRecord;
public record CreateProjectDto([Required] string Name, [Required] string Description, string userId);
public record ProjectUpdateDto([Required] string Name, [Required] string Description, DateTime UpdatedAt);
