using ProjectManagement.Services.Domain.Task.Dto;
using ProjectManagement.Services.Domain.User.Dto;

namespace ProjectManagement.Services.Domain.ProjecT.Dto
{
    public record ProjectRecordDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskDto Task { get; set; } = null!;
        public UserModelDto User { get; set; } = null!;
    }
}
