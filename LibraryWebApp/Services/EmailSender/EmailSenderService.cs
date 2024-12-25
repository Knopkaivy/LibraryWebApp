using System.Net;
using System.Net.Mail;

namespace LibraryWebApp.Services.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("tetianaivy@gmail.com", "umuc pjui reto sjxb")
            };

            return client.SendMailAsync(
                new MailMessage(from: "tetianaivy@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
