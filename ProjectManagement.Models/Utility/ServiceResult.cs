using System.Net;

namespace ProjectManagement.Models.Utility;

public abstract class ServiceResult
{
    public bool IsSuccessful { get; set; }
    public string? Message { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
}
