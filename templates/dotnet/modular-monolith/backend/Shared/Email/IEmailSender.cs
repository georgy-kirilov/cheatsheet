namespace Shared.Email;

public interface IEmailSender
{
    Task<bool> SendAsync(
        string fromEmail,
        string fromName,
        string toEmail,
        string toName,
        string subject,
        string htmlContent);
}
