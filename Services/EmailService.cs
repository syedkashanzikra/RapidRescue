using System.Net.Mail;
using System.Net;

namespace RapidRescue.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSenderEmail()
        {
            return _configuration["EmailSettings:SenderEmail"];
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var smtpClient = new SmtpClient
            {
                Host = emailSettings["SmtpServer"],
                Port = int.Parse(emailSettings["SmtpPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"])
            };

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
