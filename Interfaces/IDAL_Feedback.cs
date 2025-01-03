using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Feedback
    {
        List<Feedback> GetAllFeedbacks(int product_Id);
        void AddOrUpdateFeedback(Feedback feedback);
        void DeleteFeedback(Feedback feedback);
    }
}