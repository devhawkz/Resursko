using MimeKit;
using MailKit.Net.Smtp;

namespace Resursko.API.Services.EmailService;

public class EmailSenderAsync(EmailConfiguration emailConfiguration) : IEmailSenderAsync
{
    public async Task SendEmailAsync(Message message)
    {
        var emailMessage = CreateEmailMessage(message);

        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Resursko", emailConfiguration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content};

        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using(var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, true);

                // removed in order to use our authentication
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailConfiguration.Username, emailConfiguration.Password);

                await client.SendAsync(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }

}
