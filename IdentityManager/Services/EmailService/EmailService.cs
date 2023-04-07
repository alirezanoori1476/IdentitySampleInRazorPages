namespace IdentityManager.Services.EmailService;

public class EmailService : IEmailService
{
    public Task SendEmail(string email, string message)
    {
        Console.WriteLine("Send Email"); //TODO: Complete Email Service by your self.

        return Task.CompletedTask;
    }
}