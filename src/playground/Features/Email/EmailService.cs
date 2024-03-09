using System.Net.Mail;
using System.Net;
using FluentEmail.Core;
using FluentEmail.Smtp;
using FluentEmail.Razor;

namespace playground.Features.Email
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }


        public async Task SendEmailAsync(string emailAddress, string subject, string message)
        {
            using (var client = new SmtpClient("smtp.imitate.email", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("f1972196-29c7-406c-9f26-018e20df91d9", "b12f7104-c696-4669-bd95-511ed24617dd");
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("no-reply@paulmcgann83-personal-nxfj.imitate.email"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(emailAddress));

                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendEmailFluentAsync(string emailAddress, string subject)
        {
            var template = "Dear @Model.Name, You are totally @Model.Compliment.";

            await _fluentEmail
                .To(emailAddress)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "Eric", Compliment = "Awesome" })
                .SendAsync();
        }
    }
}
