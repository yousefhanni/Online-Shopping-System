using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MyShop.Utilities.Settings;
using System.Net;
using System.Net.Mail;

namespace MyShop.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MailSettings _mailSettings;

        public EmailSender(IWebHostEnvironment webHostEnvironment,
            IOptions<MailSettings> mailSettings)
        {
            _webHostEnvironment = webHostEnvironment;
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                MailMessage message = new()
                {
                    From = new MailAddress(_mailSettings.Email!, _mailSettings.DisplayName),
                    Body = htmlMessage,
                    Subject = subject,
                    IsBodyHtml = true
                };

                message.To.Add(email);

                using SmtpClient smtpClient = new(_mailSettings.Host)
                {
                    Port = _mailSettings.Port,
                    Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }



    }
}