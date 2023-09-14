using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Controllers.Shared;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domains.Auth;
using ProjectManagement.Services.Domains.Auth.Dtos;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.API.Controllers;
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthenticationService _authService;
    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Handles new user account creation
    /// </summary>
    /// <param name="model" <see cref="SignUpDto"/>></param>
    /// <returns></returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<SignedInDto>))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto model)
    {
        ServiceResponse<SignedInDto> response = await _authService.SignUpAsync(model, CancellationToken.None);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Generates a JWT and Refresh token for a user
    /// </summary>
    /// <param name="model" <see cref="SignInDto"/>></param>
    /// <returns></returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<SignedInDto>))]
    [ProducesResponseType(404, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> SignIn([FromBody] SignInDto model)
    {
        ServiceResponse<SignedInDto> response = await _authService.SignIn(model, CancellationToken.None);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Sends a password reset OTP to the user's email
    /// </summary>
    /// <param name="model"<see cref="ForgotPasswordOtpDto"/>></param>
    /// <returns></returns>
    [HttpPost("forgot-password-otp")]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    [ProducesResponseType(404, Type = typeof(ApiResponse))]
    public async Task<IActionResult> ForgotPasswordOtp([FromBody] ForgotPasswordOtpDto model)
    {
        ServiceResponse response = await _authService.SendForgotPasswordOtpAsync(model, CancellationToken.None);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Utilizes the password reset OTP in changing a users password
    /// </summary>
    /// <param name="model" <see cref="ResetPasswordDto"/>></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    [ProducesResponseType(404, Type = typeof(ApiResponse))]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        ServiceResponse response = await _authService.ResetPasswordAsync(model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Sends an email confirmation OTP to the user's stipulated email address
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("verify-email-otp")]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    [ProducesResponseType(404, Type = typeof(ApiResponse))]
    public async Task<IActionResult> VerifyEmailOtp([FromBody] VerifyEmailOtpDto model)
    {
        ServiceResponse response = await _authService.SendEmailConfirmationOtpAsync(model, CancellationToken.None);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Utilizes the email confirmation OTP in confirming the users email
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("verify-email")]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    [ProducesResponseType(404, Type = typeof(ApiResponse))]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto model)
    {
        ServiceResponse response = await _authService.VerifyEmailAsync(model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Generates a new JWT for a user using the expired JWT and the users refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("refresh")]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]
    [ProducesResponseType(404, Type = typeof(ApiResponse))]
    [ProducesResponseType(400, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Refresh([FromQuery] string accessToken, [FromQuery] string refreshToken)
    {
        ServiceResponse<SignedInDto> tokenResponse = await _authService.RefreshAccessTokenAsync(accessToken, refreshToken);
        return ComputeResponse(tokenResponse);
    }
}
