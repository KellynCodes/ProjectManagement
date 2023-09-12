using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domain.Task.Dto;

public record TaskDto([Required] string Title, [Required] string Description, [Required] DateTimeOffset DueDate, [Required] Status Status, [Required] Priority Priority) : BaseRecord;