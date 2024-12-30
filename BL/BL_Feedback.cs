using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.BL
{
    public class BL_Feedback : IBL_Feedback
    {
        private readonly IDAL_Feedback _dAL_Feedback;

        public BL_Feedback(IDAL_Feedback dAL_Feedback)
        {
            _dAL_Feedback = dAL_Feedback;
        }

        public void AddOrUpdateFeedback(Feedback feedback)
        {
            _dAL_Feedback.AddOrUpdateFeedback(feedback);
        }

        public List<Feedback> GetAllFeedbacks(int product_Id)
        {
            return _dAL_Feedback.GetAllFeedbacks(product_Id);
        }
    }
}