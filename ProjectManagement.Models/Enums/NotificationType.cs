namespace ProjectManagement.Models.Domains.Security.Enums;

public enum NotificationType
{
    /// <summary>
    /// An operation to confirma users email via 2FA OTP code
    /// </summary>
    EmailConfirmation = 1,
    /// <summary>
    /// An operation to reset a users password via 2FA OTP code
    /// </summary>
    PasswordReset,

    /// <summary>
    /// Notification for task due date reminder
    /// </summary>
    DueDateReminder,

    /// <summary>
    /// Notification for task status update
    /// </summary>
    StatusUpdate,
}
