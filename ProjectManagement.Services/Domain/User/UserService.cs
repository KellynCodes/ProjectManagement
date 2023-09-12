using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManagement.Data.Interfaces;
using ProjectManagement.Models.Identity;
using ProjectManagement.Models.Utility;
using ProjectManagement.Services.Domain.ProjecT.Dto;
using ProjectManagement.Services.Domain.User.Dto;
using ProjectManagement.Services.Utility;
using System.Net;

namespace ProjectManagement.Services.Domain.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ApplicationUser> _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _userRepo = _unitOfWork.GetRepository<ApplicationUser>();
        }
        public async Task<ServiceResponse<UserDto>> GetUserAsync(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            UserDto result = _mapper.Map<UserDto>(user);
            return new ServiceResponse<UserDto>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} was found.",
            };
        }


        public async Task<ServiceResponse<PaginationResponse<UserRecordDto>>> GetUsersAsync(RequestParameters requestParameters)
        {
            try
            {
                PaginationResult<ApplicationUser> users = await _userRepo.GetPagedItems(
                     requestParameters,
                      predicate: x => !x.DeActivated,
                      orderBy: o => o.OrderBy(x => x.Name.Contains(requestParameters.Keywords)),
                      include: inc => inc.Include(x => x.Projects.Select(x => new ProjectDto
                      (
                          x.Id,
                          x.Name,
                          x.Description,
                          x.CreatedAt
                      )))
                     );
                IEnumerable<UserRecordDto> userRecord = users.Records.Select(u => new UserRecordDto
                (
                    u.Name,
                    u.UserName!,
                    u.Projects!
                ));

                var result = new PaginationResponse<UserRecordDto>
                (
                    PageSize: users.PageSize,
                    CurrentPage: users.CurrentPage,
                    TotalPages: users.TotalPages,
                    TotalRecords: users.TotalRecords,
                    Records: userRecord
                );
                return new ServiceResponse<PaginationResponse<UserRecordDto>>
                {
                    Data = result,
                    Message = $"{result.TotalRecords} was found.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Users not fetched; Reason: {ex.Message}");
                throw new InvalidOperationException($"Users not fetched; Reason: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<UserDto>> DeleteAccountAsync(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }
            IdentityResult response = await _userManager.DeleteAsync(user);
            if (!response.Succeeded)
            {
                string? message = response.Errors.Select(e => e.Description).FirstOrDefault();
                return new ServiceResponse<UserDto>
                {
                    Message = $"Operation failed with reason: {message}",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return new ServiceResponse<UserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} was deleted.",
            };
        }

        public async Task<ServiceResponse<UserDto>> UpdateUserAsync(string userId, UserDto model)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }
            user = _mapper.Map<ApplicationUser>(model);
            IdentityResult response = await _userManager.UpdateAsync(user);
            if (!response.Succeeded)
            {
                string? message = response.Errors.Select(e => e.Description).FirstOrDefault();
                return new ServiceResponse<UserDto>
                {
                    Message = $"Operation failed with reason: {message}",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return new ServiceResponse<UserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} your account was updated successfully.",
            };
        }

        public async Task<ServiceResponse<UserDto>> UpdateUserAsync(string userId, JsonPatchDocument<UserDto> model)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var userToPatch = new UserDto(user.Name, user.UserName!);
            model.ApplyTo(userToPatch);
            user = new ApplicationUser
            {
                Name = userToPatch.Name,
                UserName = userToPatch.UserName,
            };
            IdentityResult response = await _userManager.UpdateAsync(user);
            if (!response.Succeeded)
            {
                string? message = response.Errors.Select(e => e.Description).FirstOrDefault();
                return new ServiceResponse<UserDto>
                {
                    Message = $"Operation failed with reason: {message}",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return new ServiceResponse<UserDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} your account was updated successfully.",
            };
        }
    }
}
