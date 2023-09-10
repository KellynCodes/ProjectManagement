using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domains.Auth.Dtos;

/// <summary>
/// Client request payload for the creation of new accounts
/// </summary>
/// <param name="Email"></param>
/// <param name="Username"></param>
/// <param name="Password"></param>
/// <param name="ConfirmPassword"></param>
public record SignUpDto([Required] string Email, [Required] string Username, [Required] string Password, [Required] string ConfirmPassword);
