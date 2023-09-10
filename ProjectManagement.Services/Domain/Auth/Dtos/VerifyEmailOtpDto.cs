using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domains.Auth.Dtos;
/// <summary>
/// Client request payload used in the sending of 2FA OTP for email confirmation
/// </summary>
/// <param name="Email"></param>
public record VerifyEmailOtpDto([Required] string Email);

