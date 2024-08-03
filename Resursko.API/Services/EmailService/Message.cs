using MimeKit;

namespace Resursko.API.Services.EmailService;

public class Message
{
    public List<MailboxAddress> To {  get; set; } = new List<MailboxAddress>();
    public string Subject { get; set; }
    public string Content {  get; set; }

    public Message(IEnumerable<string> to, string subject, string content)
    {
        To.AddRange(to.Select(x => new MailboxAddress(x)));
        Subject = subject;
        Content = content;
    }
}
