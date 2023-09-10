using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domains.Auth.Dtos;
/// <summary>
/// Client request payload for the sending of 2FA OTP used in resetting of users password
/// </summary>
/// <param name="Email"></param>
public record ForgotPasswordOtpDto([Required] string Email);
