using Microsoft.AspNetCore.Identity;
using ProjectManagement.Models.Domains.User.Enums;
using ProjectManagement.Models.Entities.Domains.Project;

namespace ProjectManagement.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public bool DeActivated { get; set; }
    public UserRole UserRole { get; set; }
    public Guid? ProjectId { get; set; }
    public virtual ICollection<Project> Projects { get; set; } = null!;
}
