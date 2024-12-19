using NHibernate;
using NHibernate.Linq;
using Shopsy_Project.Common;
using Shopsy_Project.Extensions;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopsy_Project.DAL
{
    public class DAL_Cart : IDAL_Cart
    {
        private readonly ISessionFactory _sessionFactory;

        public DAL_Cart()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        public void AddCartItems(AddCartRequest cartRequest)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var UId = session.Query<Cart>().Where(x => x.Quantity > 0 && x.userId == cartRequest.userId).FirstOrDefault();
                        if (UId != null)
                        {
                            throw new ArgumentException("Cart already exists..");
                        }
                        ////var IsCartExisting = session.Query<Cart>().Where(x=>x.userId == cartRequest.userId);
                        ////cartlist = session.Query<Cart>().Where(x => x.Quantity > 0).ToList();
                        //if (IsCartExisting != null)
                        //{
                        //    throw new ArgumentException("User cart already exists..");
                        //}

                        var cart = new Cart
                        {
                            userId = cartRequest.userId,
                            product_Id = cartRequest.product_Id,
                            Quantity = cartRequest.Quantity,
                            DateCreated = cartRequest.DateCreated
                        };

                        session.Save(cart);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new ArgumentException("Error while inserting records.", ex);
                    }
                }
            }
        }

        public void DeleteCartItems(int cartId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var IsCartExisting = session.Get<Cart>(cartId);
                        if (IsCartExisting == null)
                        {
                            throw new ArgumentException("User cart already deleted or not exists..");
                        }

                        session.Delete(IsCartExisting);
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

        public List<Cart> GetAllCartItems()
        {
            var cartlist = new List<Cart>();
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        cartlist = session.Query<Cart>().Where(x => x.Quantity > 0).ToList();
                        foreach (var item in cartlist)
                        {
                            //var cart = new Cart();
                            item.user = session.Query<Users>().FirstOrDefault(u => u.Id == item.userId);
                            item.products = session.Query<Products>().Where(u => u.Product_Id == item.product_Id).ToList();
                        }

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new ArgumentException("Error while fetching records.", ex);
                    }
                }
            }
            return cartlist;
        }

        public Cart GetCartById(int Id)
        {
            Cart cart = new Cart();
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        cart = session.Query<Cart>().FirstOrDefault(u => u.Cart_Id == Id);
                        if (cart == null)
                        {
                            throw new ArgumentException("User not exists for this cart..Invalid cart..");
                        }
                        cart.user = session.Query<Users>().FirstOrDefault(u => u.Id == cart.userId);
                        cart.products = session.Query<Products>().Where(u => u.Product_Id == cart.product_Id).ToList();
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new ArgumentException("Error while fetching records.", ex);
                    }
                }
            }
            return cart ?? new Cart();
        }

        public void UpdateCartItems(Cart cart)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        var existingCart = session.Get<Cart>(cart.Cart_Id);
                        if (existingCart != null)
                        {
                            existingCart.product_Id = cart.product_Id;
                            existingCart.Cart_Id = cart.Cart_Id;
                            existingCart.DateCreated = cart.DateCreated;
                            existingCart.Quantity = cart.Quantity;

                            session.Update(existingCart);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid Cart product or product not present in your cart.");
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
    }
}