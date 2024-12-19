//using NHibernate;
//using Shopsy_Project.Common;
//using Shopsy_Project.Interfaces;
//using Shopsy_Project.Models;
//using Shopsy_Project.Models.RequestModels;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace Shopsy_Project.DAL
//{
//    public class DAL_Cart
//    {
//        private readonly ISessionFactory _sessionFactory;

//        public DAL_Cart()
//        {
//            var config = ConfigurationManager.SetConfiguration();
//            _sessionFactory = config.BuildSessionFactory();
//        }

//        public void AddCartItems(AddCartRequest cartRequest)
//        {
//            using (var session = _sessionFactory.OpenSession())
//            using (var tx = session.BeginTransaction())
//            {
//                try
//                {
//                    var query = session.CreateSQLQuery("EXEC AddCartItems :userId, :productId, :quantity, :dateCreated");
//                    query.SetParameter("userId", cartRequest.userId, NHibernateUtil.Int32);
//                    query.SetParameter("productId", cartRequest.product_Id, NHibernateUtil.Int32);
//                    query.SetParameter("quantity", cartRequest.Quantity, NHibernateUtil.Int32);
//                    query.SetParameter("dateCreated", cartRequest.DateCreated, NHibernateUtil.DateTime);

//                    query.ExecuteUpdate();
//                    tx.Commit();
//                }
//                catch (Exception ex)
//                {
//                    tx.Rollback();
//                    throw new ArgumentException("Error while inserting records.", ex);
//                }
//            }
//        }

//        public void DeleteCartItems(int cartId)
//        {
//            using (var session = _sessionFactory.OpenSession())
//            using (var tx = session.BeginTransaction())
//            {
//                try
//                {
//                    var query = session.CreateSQLQuery("EXEC DeleteCartItem :cartId");
//                    query.SetParameter("cartId", cartId, NHibernateUtil.Int32);

//                    query.ExecuteUpdate();
//                    tx.Commit();
//                }
//                catch (Exception ex)
//                {
//                    tx.Rollback();
//                    throw new ArgumentException("Error while deleting records.", ex);
//                }
//            }
//        }

//        public List<Cart> GetAllCartItems()
//        {
//            using (var session = _sessionFactory.OpenSession())
//            {
//                try
//                {
//                    var query = session.CreateSQLQuery("EXEC GetAllCartItems")
//                        .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<Cart>());
                    
//                    var cartList = query.List<Cart>().ToList();
//                    return cartList;
//                }
//                catch (Exception ex)
//                {
//                    throw new ArgumentException("Error while fetching records.", ex);
//                }
//            }
//        }

//        public Cart GetCartById(int id)
//        {
//            using (var session = _sessionFactory.OpenSession())
//            {
//                try
//                {
//                    var query = session.CreateSQLQuery("EXEC GetCartById :id")
//                        .SetParameter("id", id, NHibernateUtil.Int32)
//                        .SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<Cart>());

//                    var cart = query.UniqueResult<Cart>();
//                    if (cart == null)
//                        throw new ArgumentException("Invalid cart ID or cart does not exist.");

//                    return cart;
//                }
//                catch (Exception ex)
//                {
//                    throw new ArgumentException("Error while fetching records.", ex);
//                }
//            }
//        }

//        public void UpdateCartItems(Cart cart)
//        {
//            using (var session = _sessionFactory.OpenSession())
//            using (var tx = session.BeginTransaction())
//            {
//                try
//                {
//                    var query = session.CreateSQLQuery("EXEC UpdateCartItems :cartId, :productId, :quantity, :dateCreated");
//                    query.SetParameter("cartId", cart.Cart_Id, NHibernateUtil.Int32);
//                    query.SetParameter("productId", cart.product_Id, NHibernateUtil.Int32);
//                    query.SetParameter("quantity", cart.Quantity, NHibernateUtil.Int32);
//                    query.SetParameter("dateCreated", cart.DateCreated, NHibernateUtil.DateTime);

//                    query.ExecuteUpdate();
//                    tx.Commit();
//                }
//                catch (Exception ex)
//                {
//                    tx.Rollback();
//                    throw new ArgumentException("Error while updating records.", ex);
//                }
//            }
//        }
//    }
//}
