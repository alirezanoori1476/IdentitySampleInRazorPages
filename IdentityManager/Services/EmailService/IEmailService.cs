namespace IdentityManager.Services.EmailService;

public interface IEmailService
{
    Task SendEmail(string email, string message);
}