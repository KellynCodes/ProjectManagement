namespace ProjectManagement.AsyncClient.Dto;

public record UploadedFileDto(bool IsSuccessful, string? Message, FileStream? Stream);
