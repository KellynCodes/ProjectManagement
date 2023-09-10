using ProjectManagement.Models.Identity;

namespace ProjectManagement.Models.Entities.Domains.Project
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public virtual ICollection<ApplicationUser> User { get; set; } = default!;
        public virtual ICollection<ProjTask> Tasks { get; set; } = default!;
    }
}
