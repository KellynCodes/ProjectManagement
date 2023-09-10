using ProjectManagement.Cache.Interfaces;
using ProjectManagement.Services.Domains.Security;
using ProjectManagement.Services.Domains.Security.Dtos;

namespace ProjectManagement.Services.Domains.Auth;
public class AccountLockoutService : IAccountLockoutService
{
    private readonly ICacheService _cacheService;
    private TimeSpan ExpirationTime = TimeSpan.FromHours(1);
    private const int MAX_RECURRENT_FAILED_SIGN_IN_ATTEMPT = 5;

    public AccountLockoutService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<bool> RecordFailedLoginAttempt(string userId)
    {
        string cacheKey = CacheKeySelector.AccountLockoutCacheKey(userId);
        AccountLockoutDto? accountLockout = await _cacheService.ReadFromCache<AccountLockoutDto>(cacheKey);
        if (accountLockout == null)
        {
            accountLockout = new AccountLockoutDto(false, 1);
        }
        else
        {
            ++accountLockout.FailedLoginAttempts;
        }

        if (accountLockout.FailedLoginAttempts >= MAX_RECURRENT_FAILED_SIGN_IN_ATTEMPT && !accountLockout.IsLocked)
        {
            accountLockout.IsLocked = true;
            accountLockout.LockedAt = DateTime.UtcNow;
        }

        await _cacheService.WriteToCache(cacheKey, accountLockout, null, ExpirationTime);
        return accountLockout.IsLocked;
    }

    public async Task RecordSuccessfulLoginAttempt(string userId)
    {
        string cacheKey = CacheKeySelector.AccountLockoutCacheKey(userId);
        AccountLockoutDto? accountLockout = await _cacheService.ReadFromCache<AccountLockoutDto>(cacheKey);
        if (accountLockout == null)
        {
            return;
        }

        await _cacheService.ClearFromCache(cacheKey);
    }

    public async Task<(bool, int?)> IsAccountLocked(string userId)
    {
        string cacheKey = CacheKeySelector.AccountLockoutCacheKey(userId);
        AccountLockoutDto? accountLockout = await _cacheService.ReadFromCache<AccountLockoutDto>(cacheKey);
        if (accountLockout == null)
        {
            return (false, null);
        }

        if (!accountLockout.IsLocked)
        {
            return (false, null);
        }

        if (accountLockout.IsLocked && !accountLockout.LockedAt.HasValue)
        {
            throw new NullReferenceException("Locked accounts must specify the lock timestamp");
        }


        DateTime expectedUnlockDate = accountLockout.LockedAt!.Value.Add(ExpirationTime);
        int minutesLeft = (int)expectedUnlockDate.Subtract(DateTime.UtcNow).TotalMinutes;
        return (true, minutesLeft);
    }

}