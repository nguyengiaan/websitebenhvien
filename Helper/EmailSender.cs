using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message, IFormFile attachment);
}

public class EmailSender : IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public EmailSender(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message, IFormFile attachment)
{
    var emailMessage = new MimeMessage();
    emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
    emailMessage.To.Add(new MailboxAddress("", email));
    emailMessage.Subject = subject;
    // Tạo body email với file đính kèm
    var bodyBuilder = new BodyBuilder
    {
        TextBody = message
    };

    // Kiểm tra nếu có file đính kèm
    if (attachment != null)
    {
        using (var stream = new MemoryStream())
        {
            await attachment.CopyToAsync(stream);
            bodyBuilder.Attachments.Add(attachment.FileName, stream.ToArray(), ContentType.Parse(attachment.ContentType));
        }
    }

    emailMessage.Body = bodyBuilder.ToMessageBody();

    using (var client = new SmtpClient())
    {
        await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}

}