using Microsoft.AspNetCore.JsonPatch;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.User.Dto;
using ProjectManagement.Services.Utility;

namespace ProjectManagement.Services.Domain.User
{
    public interface IUserService
    {
        Task<ServiceResponse<UserModelDto>> GetUserAsync(string userId);
        Task<ServiceResponse<PaginationResponse<UserRecordDto>>> GetUsersAsync(RequestParameters requestParameters);
        Task<ServiceResponse<UserModelDto>> UpdateUserAsync(string userId, UserModelDto model);
        Task<ServiceResponse<UserModelDto>> UpdateUserAsync(string userId, JsonPatchDocument<UserModelDto> model);
        Task<ServiceResponse<UserModelDto>> DeleteAccountAsync(string userId);
    }
}
