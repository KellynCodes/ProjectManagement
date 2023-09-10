
using ProjectManagement.Models.Utility;

namespace ProjectManagement.AsyncClient.Dto;
public record FileUploadedDto(bool IsSuccessful, string? Message, string? S3Path) : BaseRecord;

