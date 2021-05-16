using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Clinic
{
    public class EmailService
    {
        public  void SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "Vaikl2017@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart()
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                 client.Connect("smtp.yandex.ru", 465, true);
                 client.Authenticate("Vaikl2017@yandex.ru", "diplom1234");
                client.Send(emailMessage);

                 client.Disconnect(true);
            }
        }
    }
}
