using IdentityManager.Models;

namespace IdentityManager.Services.EmailService;

public interface IEmailService
{
    Task SendEmail(EmailModel emailModel, CancellationToken cancellationToken);
}