using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MINT.EShop.Core.Interfaces;
using MINT.EShop.Core.Options;

namespace MINT.EShop.Infrastracture
{
    public class EmailSender(IOptions<EmailOptions> emailOptions) : IEmailSender
    {
        private readonly EmailOptions _emailOptions = emailOptions.Value;
        public async Task SendVerificationCodeAsync(string targetEmail, string code)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(baseDir, "Templates", "EmailActivation.html");

            string htmlBody = await File.ReadAllTextAsync(filePath);

            htmlBody = htmlBody.Replace("{{CHECK_CODE}}", code);

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailOptions.SenderName, _emailOptions.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", targetEmail));
            emailMessage.Subject = "Verification Code for Shopik";

            var bodyBuilder = new BodyBuilder { HtmlBody =  htmlBody };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailOptions.SenderEmail, _emailOptions.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}