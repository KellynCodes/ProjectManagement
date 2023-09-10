namespace ProjectManagement.Models.Enums;
/// <summary>
/// Represents the different OTP operations we support
/// </summary>
public enum OtpOperation
{
    /// <summary>
    /// An operation to confirma users email via 2FA OTP code
    /// </summary>
    EmailConfirmation = 1,
    /// <summary>
    /// An operation to reset a users password via 2FA OTP code
    /// </summary>
    PasswordReset,
}
