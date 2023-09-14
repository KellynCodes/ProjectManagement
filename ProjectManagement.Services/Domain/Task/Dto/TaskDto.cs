using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.Task.Dto;

public record TaskDto([Required] string Title, [Required] string Description, [Required] Status Status, [Required] Priority Priority) : BaseRecord;

public record TaskRecordResponseDto : BaseRecord
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
}