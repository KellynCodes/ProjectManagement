using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;

    [Timestamp]
    public byte[]? TimeStamp { get; set; }
}


