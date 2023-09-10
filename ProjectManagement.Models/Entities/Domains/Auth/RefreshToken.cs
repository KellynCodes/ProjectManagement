using ProjectManagement.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Models.Entities.Domains.Auth;
public class RefreshToken : BaseEntity
{
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; } = null!;
}
