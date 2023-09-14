using ProjectManagement.Models.Domains.Security.Enums;

namespace ProjectManagement.Services.Domains.Notification.Dtos;
/// <summary>
/// Responsible for sending a notification command to the queue url for otp email sending
/// </summary>
/// <param name="userId">User we're sending the email to</param>
/// <param name="email">Email address of the recipient of the otp email</param>
/// <param name="fullName">Fullname of the recipient of the otp email</param>
/// <param name="ProjectId">Hostel whose subdomain from which the user is attempting to reset their password</param>
/// <param name="operation">The operation for which we are attempting to generate an otp code</param>
public record CreateOtpNotificationDto(string userId, string email, string fullName, NotificationType operation);