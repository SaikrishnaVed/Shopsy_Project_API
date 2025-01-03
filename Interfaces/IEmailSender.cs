using Shopsy_Project.Helpers;

namespace Shopsy_Project.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
