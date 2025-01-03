using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System.Drawing.Drawing2D;

namespace Shopsy_Project.DAL
{
    public class DAL_Feedback : IDAL_Feedback
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HttpContextAccessor _httpContextAccessor;
        
        public DAL_Feedback()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        //public void AddOrUpdateFeedback(Feedback feedback)
        //{
        //    using (var session = _sessionFactory.OpenSession())
        //    {
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            try
        //            {
        //                ////check whether feedback exists to update or add new for user of that product.
        //                ///// var isFeedbackExists 
        //                var feedbackId = session.Query<Feedback>().Where(x => x.product_Id == feedback.product_Id && x.userId == feedback.userId)?.FirstOrDefault()?.Id;
        //                    //!= null ? true : false;
        //                if ((feedback != null && feedback?.Id != null || feedback?.Id == 0) && (feedbackId == null || feedbackId == 0))
        //                {
        //                    feedback.Id = Convert.ToInt32(feedbackId);
        //                    session.Save(feedback);
        //                }
        //                else 
        //                {
        //                    session.Merge(feedback);
        //                }
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw new Exception("An error occurred while adding the feedback.", ex);
        //            }
        //        }
        //    }
        //}


        public void AddOrUpdateFeedback(Feedback feedback)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        if (feedback == null)
                        {
                            throw new ArgumentNullException(nameof(feedback), "Feedback cannot be null.");
                        }

                        var existingFeedback = session.Query<Feedback>()
                                                      .FirstOrDefault(x => x.product_Id == feedback.product_Id && x.userId == feedback.userId);

                        if (existingFeedback == null)
                        {
                            session.Save(feedback);
                        }
                        else
                        {
                            feedback.Id = existingFeedback.Id;
                            session.Merge(feedback);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while adding or updating the feedback.", ex);
                    }
                }
            }
        }


        public List<Feedback> GetAllFeedbacks(int product_Id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                // Fetch all feedbacks
                List<Feedback> feedbacks = session.Query<Feedback>().Where(x=>x.product_Id == product_Id).ToList();

                // Get distinct userIds from feedbacks
                List<int> allUserIds = feedbacks.Select(x => x.userId).Distinct().ToList();

                // Fetch all users in a single query
                var users = session.Query<Users>()
                                   .Where(u => allUserIds.Contains(u.Id))
                                   .ToDictionary(u => u.Id, u => u);

                // Update usernames in feedbacks
                foreach (var feedback in feedbacks)
                {
                    if (users.TryGetValue(feedback.userId, out var user))
                    {
                        feedback.username = user.Name ?? string.Empty;
                    }
                }

                return feedbacks;
            }
        }

        public void DeleteFeedback(Feedback feedback)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var IsFeedbackExisting = session.Get<Feedback>(feedback.Id);
                        if (IsFeedbackExisting == null)
                        {
                            throw new ArgumentException("User Feedback already deleted or not exists..");
                        }

                        session.Delete(IsFeedbackExisting);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new ArgumentException("Error while deleting records.", ex);
                    }
                }
            }
        }

    }
}