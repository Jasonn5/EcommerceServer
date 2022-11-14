using Authentication.Entities;
using Authentication.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Authentication.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public void SendEmail(Email email)
        {
            var mailMessage = new MailMessage
            {
                Subject = email.Subject,
                Body = email.Message,
                From = new MailAddress(
                    emailSettings.FromAddress,
                    emailSettings.FromDisplayName
                    ),
                IsBodyHtml = email.IsBodyHtml
            };

            mailMessage.To.Add(email.ToAddresses);

            if (!string.IsNullOrEmpty(email.BccAddresses))
            {
                mailMessage.Bcc.Add(email.BccAddresses);
            }

            if (!string.IsNullOrEmpty(email.CcAddresses))
            {
                mailMessage.CC.Add(email.CcAddresses);
            }

            using (var smtpClient = new SmtpClient(emailSettings.SmtpClientHost))
            {
                if (emailSettings.SmtpClientPort.HasValue)
                {
                    smtpClient.Port = emailSettings.SmtpClientPort.Value;
                }

                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(emailSettings.SmtpClientUsername, emailSettings.SmtpClientPassword);

                smtpClient.Send(mailMessage);
            }
        }
    }
}
