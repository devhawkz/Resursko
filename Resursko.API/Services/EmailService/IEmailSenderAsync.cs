namespace Resursko.API.Services.EmailService;

public interface IEmailSenderAsync
{
    Task SendEmailAsync(Message message);
}
