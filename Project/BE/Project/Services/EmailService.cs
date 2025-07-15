using Microsoft.AspNetCore.Mvc;
using Project.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Project.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public async Task SendVerificationEmailAsync(string email, string code)
        {
            var smtpClient = new SmtpClient(_configuration["SmtpSettings:Server"])
            {
                Port = int.Parse(_configuration["SmtpSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["SmtpSettings:SenderEmail"],
                    _configuration["SmtpSettings:SenderPassword"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SmtpSettings:SenderEmail"]),
                Subject = "Verification Code",
                Body = $"Your verification code is: {code}",
                To = { new MailAddress(email) }
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

