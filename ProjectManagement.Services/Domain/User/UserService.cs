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
        public async Task<ServiceResponse<UserModelDto>> GetUserAsync(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserModelDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            UserModelDto result = _mapper.Map<UserModelDto>(user);
            return new ServiceResponse<UserModelDto>
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
                      include: inc => inc.Include(x => x.Projects)
                     );
                var userRecord = users.Records.Select(u => new UserRecordDto
                (
                    u.Name,
                    u.UserName!,
                    u.Projects.Select(x => new ProjectDto(x.Id, x.UserId, x.Name, x.Description, x.CreatedAt)
                 )));

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

        public async Task<ServiceResponse<UserModelDto>> DeleteAccountAsync(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserModelDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            user.IsDeleted = true;
            await _userRepo.UpdateAsync(user);
            return new ServiceResponse<UserModelDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} was soft deleted.",
            };
        }

        public async Task<ServiceResponse<UserModelDto>> UpdateUserAsync(string userId, UserModelDto model)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserModelDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }
            _mapper.Map(model, user);
            ApplicationUser response = await _userRepo.UpdateAsync(user);
            UserModelDto result = new(response.Name, response.UserName!);
            return new ServiceResponse<UserModelDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} your account was updated successfully.",
                Data = result,
            };
        }

        public async Task<ServiceResponse<UserModelDto>> UpdateUserAsync(string userId, JsonPatchDocument<UserModelDto> model)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<UserModelDto>
                {
                    Message = "User not found.",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            var userToPatch = new UserModelDto(user.Name, user.UserName!);
            model.ApplyTo(userToPatch);
            user.Name = userToPatch.Name;
            user.UserName = userToPatch.UserName;
            ApplicationUser response = await _userRepo.UpdateAsync(user);
            UserModelDto result = new(response.Name, response.UserName!);
            return new ServiceResponse<UserModelDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = $"{user.Name} your account was updated successfully.",
                Data = result
            };
        }
    }
}
