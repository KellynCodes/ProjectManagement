namespace ProjectManagement.Models.Domains.Security.Dtos;
/// <summary>
/// Data structure to capture the generated user otp in cache
/// </summary>
public record OtpCodeDto
{
    /// <summary>
    /// Generated Otp code
    /// </summary>
    public string Otp { get; set; }
    /// <summary>
    /// Identifier of the user that generated the otp code
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// Number of failed attempts to match the correct generated otp code
    /// </summary>
    public int Attempts { get; set; }

    public OtpCodeDto(string otp, string userId)
    {
        Otp = otp;
        UserId = userId;
        Attempts = 0;
    }
}
