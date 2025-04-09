using System.Net;
using System.Net.Mail;

namespace web_api.Helper;

public static class EmailHelper
{
    private static IConfiguration? _configuration;

    public static void Initialize(IConfiguration configuration) => _configuration = configuration;

    public static async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var senderEmail = _configuration?["SenderEmail"];
        var senderPassword = _configuration?["SenderPassword"];

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}