using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domains.Auth.Dtos;
/// <summary>
/// Client request payload for the sign in action
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record SignInDto([Required] string Email, [Required] string Password);
