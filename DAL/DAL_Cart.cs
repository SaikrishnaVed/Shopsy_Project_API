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
using System.Security.Claims;

namespace Shopsy_Project.DAL
{
    public class DAL_Cart : IDAL_Cart
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public DAL_Cart()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }
        public DAL_Cart(HttpContextAccessor httpContextAccessor, ClaimsPrincipal claimsPrincipal)
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
            _httpContextAccessor = httpContextAccessor;
            _claimsPrincipal = claimsPrincipal;
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

                        //if (UId != null)
                        //{
                        //    throw new ArgumentException("Cart already exists..");
                        //}

                        ////var IsCartExisting = session.Query<Cart>().Where(x=>x.userId == cartRequest.userId);
                        ////cartlist = session.Query<Cart>().Where(x => x.Quantity > 0).ToList();
                        //if (IsCartExisting != null)
                        //{
                        //    throw new ArgumentException("User cart already exists..");
                        //}

                        var cart = new Cart
                        {
                            Cart_Id = cartRequest.userId,
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

        public List<Cart> GetAllCartItems(int userId)
        {
            //var UserId = ClaimsPrincipalExtensions.GetUserId(_claimsPrincipal);
            var cartlist = new List<Cart>();
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    try
                    {
                        cartlist = session.Query<Cart>().Where(x => x.Quantity > 0).ToList();
                        //int userId = session.Query<Users>().Select(x=>x.Id).FirstOrDefault();
                        cartlist = cartlist.Where(x => x.userId == userId).ToList();
                        foreach (var item in cartlist)
                        {
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

                        var cartlist = session.Query<Cart>().Where(x => x.Quantity > 0).ToList();
                        //int userId = session.Query<Users>().Select(x=>x.Id).FirstOrDefault();

                        if (cartlist.Where(x => x.product_Id == cart.product_Id && x.userId == cart.userId)?.FirstOrDefault() != null)
                        {
                            Cart c = cartlist.Where(x => x.product_Id == cart.product_Id && x.userId == cart.userId)?.FirstOrDefault();

                            //if(cart.Quantity <= 0)
                            //{
                            //    this.DeleteCartItems(c.Cart_Id);
                            //}
                            cart.Cart_Id = c != null ? c.Cart_Id : 0;
                        //else { 
                            if (c != null)
                            {
                                Cart existingCart = new Cart();

                                existingCart.product_Id = cart.product_Id;
                                existingCart.Cart_Id = c.Cart_Id;
                                existingCart.DateCreated = DateTime.UtcNow;
                                existingCart.Quantity = cart.Quantity;
                                existingCart.userId = cart.userId;

                                session.Merge(existingCart);
                            }
                        //}

                        tx.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        if(cart.Quantity == 0 && cart.Cart_Id > 0)
                        {
                            this.DeleteCartItems(cart.Cart_Id);
                        }
                        tx.Rollback();
                    }
                }
            }
            
        }
    }
}