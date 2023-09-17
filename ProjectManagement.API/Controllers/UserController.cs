using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Controllers.Shared;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.User;
using ProjectManagement.Services.Domain.User.Dto;

namespace ProjectManagement.API.Controllers;
[Route("api/v{version:apiVersion}/user")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class UserController : BaseController
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get single user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] string userId)
    {
        var response = await _userService.GetUserAsync(userId);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Get all user
    /// </summary>
    /// <param name="requestParameters"></param>
    /// <returns></returns>
    [HttpGet("get-all")]
    public async Task<IActionResult> GetUser([FromQuery] RequestParameters requestParameters)
    {
        var response = await _userService.GetUsersAsync(requestParameters);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAccount([FromRoute] string userId, [FromBody] UserModelDto model)
    {
        var response = await _userService.UpdateUserAsync(userId, model);
        return ComputeResponse(response);
    }

    /// <summary>
    /// Update user by patch
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch("{userId}")]
    public async Task<IActionResult> UpdateAccount([FromRoute] string userId, [FromBody] JsonPatchDocument<UserModelDto> model)
    {
        var response = await _userService.UpdateUserAsync(userId, model);
        return ComputeResponse(response);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string userId)
    {
        var response = await _userService.DeleteAccountAsync(userId);
        return ComputeResponse(response);
    }
}
