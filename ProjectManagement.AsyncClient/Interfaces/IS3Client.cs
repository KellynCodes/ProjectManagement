using ProjectManagement.AsyncClient.Dto;

namespace ProjectManagement.AsyncClient.Interfaces;

public interface IS3Client
{
    Task<UploadedFileDto> GetFile(string bucketName, string keyName);
    Task<FileUploadedDto> UploadFile(UploadFileDto model);
}
