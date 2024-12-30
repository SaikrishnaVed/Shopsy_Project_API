using NHibernate;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using NHibernate.Linq;
using System.Web.Http.ModelBinding;

namespace Shopsy_Project.DAL
{
    public class DAL_WishItem : IDAL_WishItem
    {
        private ISessionFactory sessionFactory;

        public DAL_WishItem()
        {
            var config = ConfigurationManager.SetConfiguration();
            sessionFactory = config.BuildSessionFactory();
        }

        public void AddWishItem(WishItem wishItem)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var userWishItem = session.Query<WishItem>()?.FirstOrDefault(a=>a.userId == wishItem.userId && a.productid == wishItem.productid);
                        if (userWishItem != null)
                        {
                            if(wishItem.Id == 0)
                            {
                                var WishItemId = session.Query<WishItem>().Where(x=>x.userId == wishItem.userId && x.productid == wishItem.productid).FirstOrDefault()?.Id;
                                wishItem.Id = Convert.ToInt32(WishItemId);
                            }
                            session.Merge(wishItem);
                        }
                        else
                        {
                            session.Save(wishItem);
                        }
                            tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
        }

        public void DeleteWishItem(int id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var wishItem = session.Get<WishItem>(id);
                        if (wishItem != null)
                        {
                            session.Delete(wishItem);
                        }
                        else
                        {
                            throw new ArgumentException("wishItem not exists to delete.");
;                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
        }

        public List<WishItem> WishItemList()
        {
            var wishItemList = new List<WishItem>();
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        wishItemList = session.Query<WishItem>().ToList();
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                    }
                }
            }
            return wishItemList;
        }
    }
}