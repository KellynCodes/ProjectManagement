using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using ProjectManagement.Models.Utility;

namespace ProjectManagement.AsyncClient.Dto
{
    public record UploadFileDto(string BucketName, string KeyName, IFormFile File, List<Tag>? tags) : BaseRecord;
}
