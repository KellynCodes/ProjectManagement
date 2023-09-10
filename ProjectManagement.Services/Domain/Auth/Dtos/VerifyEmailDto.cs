using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domains.Auth.Dtos;
/// <summary>
/// Client request payload that handles the confirmation of a users email using a 2FA OTP
/// </summary>
/// <param name="Email"></param>
/// <param name="Otp"></param>
public record VerifyEmailDto([Required] string Email, [Required] string Otp);