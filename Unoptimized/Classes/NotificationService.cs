

using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace Original.Classes
{
    public class NotificationService
    {
        private SmtpClient _client;
        private string _clientAddress = "researchbuddy3@gmail.com";

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine($"{token} Send cancelled");
            }

            if (e.Error != null)
            {
                Console.WriteLine($"{token} {e.Error.ToString()}");
            }
            else
            {
                Console.WriteLine("Message sent");
            }

        }

        public NotificationService()
        {
            _client = new SmtpClient("smtp.gmail.com");
            _client.Port = 587;
            _client.EnableSsl = true;
            _client.Credentials = new NetworkCredential(_clientAddress, "ixamrnmusolvjvgi");
        }

        public async void NotifyAcceptance(string emailAddress, string message)
        {
            var sender = new MailAddress(_clientAddress);
            var recipient = new MailAddress(emailAddress);
            var mailMessage = new MailMessage(sender, recipient);
            mailMessage.Body = message;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Subject = "Your submission has been accepted.";

            _client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            _client.SendAsync(mailMessage, "acceptance message");

            Console.WriteLine(message);
        }

        public async void NotifyRejection(string emailAddress, string message)
        {
            var sender = new MailAddress(_clientAddress);
            var recipient = new MailAddress(emailAddress);
            var mailMessage = new MailMessage(sender, recipient);
            mailMessage.Body = message;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Subject = "Your submission has been rejected.";

            _client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            _client.SendAsync(mailMessage, "rejection message");
            Console.WriteLine(message);
        }

        public async void NotifyRevision(string emailAddress, string message)
        {
            var sender = new MailAddress(_clientAddress);
            var recipient = new MailAddress(emailAddress);
            var mailMessage = new MailMessage(sender, recipient);
            mailMessage.Body = message;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Subject = "Your submission needs revisions.";

            _client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            _client.SendAsync(mailMessage, "revision message");
            Console.WriteLine(message);
        }
    }
}
