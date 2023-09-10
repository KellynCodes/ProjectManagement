namespace ProjectManagement.Services.Domains.Auth;
/// <summary>
/// Responsible for tracking failed login attempts and temporarily locking users out of their accounts
/// </summary>
public interface IAccountLockoutService
{
    /// <summary>
    /// Checks if a user's account is currently locked
    /// </summary>
    /// <param name="userId">unique identifier of a user</param>
    /// <returns></returns>
    Task<(bool, int?)> IsAccountLocked(string userId);

    /// <summary>
    /// Tracks a failed login attempt on a users account
    /// </summary>
    /// <param name="userId">unique identifier of a user</param>
    /// <returns></returns>
    Task<bool> RecordFailedLoginAttempt(string userId);

    /// <summary>
    /// Records a successful login attempt on a users account and clears any account lockout flags
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task RecordSuccessfulLoginAttempt(string userId);
}