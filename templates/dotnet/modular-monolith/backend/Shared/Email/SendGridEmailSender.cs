using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Shared.Email;

public sealed class SendGridEmailSender(
    SendGridSettings settings,
    ILogger<SendGridEmailSender> logger) : IEmailSender
{
    public async Task<bool> SendAsync(
        string fromEmail,
        string fromName,
        string toEmail,
        string toName,
        string subject,
        string htmlContent)
    {
        var client = new SendGridClient(settings.ApiKey);
        var from = new EmailAddress(fromEmail, fromName);
        var to = new EmailAddress(toEmail, toName);
        var message = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
        var response = await client.SendEmailAsync(message);
        
        if (!response.IsSuccessStatusCode)
        {
            logger.LogCritical(
                """
                Failed to send an email.
                Response status code: {ResponseStatusCode}
                Response headers: {@ResponseHeaders}
                Response body: {ResponseBody}
                Request parameters: {@RequestParameters}
                """,
                response.StatusCode,
                response.DeserializeResponseHeaders(),
                await response.Body.ReadAsStringAsync(),
                new
                {
                    fromEmail,
                    fromName,
                    toEmail,
                    toName,
                    subject
                }
            );

            return false;
        }

        return true;
    }
}
