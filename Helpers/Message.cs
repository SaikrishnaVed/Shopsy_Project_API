using MimeKit;

namespace Shopsy_Project.Helpers
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            // Add email addresses with an empty name if none is provided
            To.AddRange(to.Select(email => new MailboxAddress(string.Empty, email)));
            Subject = subject;
            Content = content;
        }
    }
}
