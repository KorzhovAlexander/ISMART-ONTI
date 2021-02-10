using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace AppExample.WebUI.Services
{
    public class EmailSenderService
    {
        /// <summary>
        /// Отправка подтверждающего письма
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="subject">Тема</param>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Houston", "email.for.testing.v1@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new BodyBuilder
                {
                    HtmlBody = message
                }
                .ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                //ToDo: нужно указать пароль)
                await client.AuthenticateAsync("email.for.testing.v1@gmail.com", "пароль");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}