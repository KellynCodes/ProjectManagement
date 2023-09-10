using ProjectManagement.Models.Utility;

namespace ProjectManagement.Services.Domains.Auth.Dtos;

public record SignedInDto(string AccessToken, string RefreshToken, long ExpiryTimeStamp):BaseRecord;
