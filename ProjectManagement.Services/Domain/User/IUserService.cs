using Microsoft.AspNetCore.JsonPatch;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.User.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.Services.Domain.User
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDto>> GetUserAsync(string userId);
        Task<ServiceResponse<PaginationResponse<UserRecordDto>>> GetUsersAsync(RequestParameters requestParameters);
        Task<ServiceResponse<UserDto>> UpdateUserAsync(string userId, UserDto model);
        Task<ServiceResponse<UserDto>> UpdateUserAsync(string userId, JsonPatchDocument<UserDto> model);
        Task<ServiceResponse<UserDto>> DeleteAccountAsync(string userId);
    }
}
