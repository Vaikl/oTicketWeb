using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Clinic
{
    public class EmailService
    {
        private string authAdress = "Vaikl2017@yandex.ru";
        private string authPass = "diplom1234";
        public  void SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", authAdress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart()
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                 client.Connect("smtp.yandex.ru", 465, true);
                 client.Authenticate(authAdress, authPass);
                client.Send(emailMessage);

                 client.Disconnect(true);
            }
        }
    }
}
