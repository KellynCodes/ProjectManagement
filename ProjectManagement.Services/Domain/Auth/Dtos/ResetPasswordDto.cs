using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Domains.Auth.Dtos;
public record ResetPasswordDto([Required] string Email, [Required] string Password, [Required] string ConfirmPassword, [Required] string Otp);