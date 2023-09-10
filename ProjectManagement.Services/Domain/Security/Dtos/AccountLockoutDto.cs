namespace ProjectManagement.Services.Domains.Security.Dtos;
/// <summary>
/// Structure stored in cache which tracks failed login attempts
/// </summary>
/// <param name="IsLocked"></param>
/// <param name="FailedLoginAttempts"></param>
public record AccountLockoutDto
{
    /// <summary>
    /// Flag that denotes when a user account is locked
    /// </summary>
    public bool IsLocked { get; set; }
    /// <summary>
    /// Counter that tracks recurrent failed login attempts by a user
    /// </summary>
    public int FailedLoginAttempts { get; set; }
    /// <summary>
    /// Timestamp when a users account was locked
    /// </summary>
    public DateTime? LockedAt { get; set; }

    public AccountLockoutDto(bool isLocked, int failedLoginAttempts)
    {
        IsLocked = isLocked;
        FailedLoginAttempts = failedLoginAttempts;
    }
}