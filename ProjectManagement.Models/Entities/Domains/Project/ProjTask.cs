using ProjectManagement.Models.Enums;
using ProjectManagement.Models.Identity;

namespace ProjectManagement.Models.Entities.Domains.Project
{
    public class ProjTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Guid? ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
