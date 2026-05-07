namespace Unoptimized.Classes
{
    public class NotificationService
    {
        public async void NotifyAcceptance(string emailAddress, string message)
        {
            Console.WriteLine(message);
        }

        public async void NotifyRejection(string emailAddress, string message)
        {
            Console.WriteLine(message);
        }

        public async void NotifyRevision(string emailAddress, string message)
        {
            Console.WriteLine(message);
        }
    }
}
