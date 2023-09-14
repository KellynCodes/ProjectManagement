using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.User.Dto;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.ProjecT.Dto;
public record ProjectDto([Required] Guid Id, [Required] string UserId, [Required] string Name, [Required] string Description, DateTime CreatedAt) : BaseRecord;
public record CreateProjectDto([Required] string Name, [Required] string Description, string userId);
public record ProjectUpdateDto([Required] string Name, [Required] string Description, DateTime UpdatedAt);

public record ProjectRecordResponseDto : BaseRecord
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserResponseDto User { get; set; }
}