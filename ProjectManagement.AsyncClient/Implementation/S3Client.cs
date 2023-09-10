using Amazon.S3;
using Amazon.S3.Model;
using ProjectManagement.AsyncClient.Dto;
using ProjectManagement.AsyncClient.Interfaces;
using System.Net;

namespace ProjectManagement.AsyncClient.Implementation;

public class S3Client : IS3Client
{
    public static string BucketPrefix = "s3://";
    private readonly IAmazonS3 _s3;

    public S3Client(IAmazonS3 s3)
    {
        _s3 = s3;
    }

    public async Task<FileUploadedDto> UploadFile(UploadFileDto model)
    {

        var uploadRequest = new PutObjectRequest()
        {
            BucketName = model.BucketName,
            Key = model.KeyName,
            InputStream = model.File.OpenReadStream(),

        };

        if (model.tags != null)
        {
            uploadRequest.TagSet = model.tags;
        }

        var putResponse = await _s3.PutObjectAsync(uploadRequest);

        if (putResponse.HttpStatusCode != HttpStatusCode.Created)
        {
            return new FileUploadedDto(false, $"Upload Failed With A Status Code Of {putResponse.HttpStatusCode}", null);
        }

        return new FileUploadedDto(true, null, $"{BucketPrefix}{model.BucketName}/{model.KeyName}");

    }

    public async Task<UploadedFileDto> GetFile(string bucketName, string keyName)
    {
        var getObjectRequest = new GetObjectRequest()
        {
            BucketName = bucketName,
            Key = keyName
        };

        var response = await _s3.GetObjectAsync(getObjectRequest);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            return new UploadedFileDto(false, $"Retrieval Of File Failed With A Status Code Of {response.HttpStatusCode}", null);
        }


        using var memoryStream = new MemoryStream();
        response.ResponseStream.CopyTo(memoryStream);

        using (FileStream file = new FileStream(keyName, FileMode.Create, FileAccess.Write))
        {
            memoryStream.WriteTo(file);
            return new UploadedFileDto(true, null, file);
        }
    }
}
