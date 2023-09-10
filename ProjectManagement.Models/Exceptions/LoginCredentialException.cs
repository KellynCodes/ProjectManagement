namespace ProjectManagement.Models.Exceptions;
public class LoginCredentialException : Exception
{
    public LoginCredentialException(string? message) : base(message)
    {
    }
}
