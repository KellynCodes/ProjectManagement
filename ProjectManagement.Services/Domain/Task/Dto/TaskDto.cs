using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Utility;

namespace ProjectManagement.Services.Domain.Task.Dto;

public record TaskDto(string Title, string Description, DateTimeOffset DueDate, Status Status, Priority Priority) : BaseRecord;