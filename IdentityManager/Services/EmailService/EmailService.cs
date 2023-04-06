using IdentityManager.Models;

namespace IdentityManager.Services.EmailService;

public class EmailService : IEmailService
{
    public Task SendEmail(EmailModel emailModel, CancellationToken cancellationToken)
    {
        Console.WriteLine("Send Email"); //TODO: Complete Email Service by your self.

        return Task.CompletedTask;
    }
}