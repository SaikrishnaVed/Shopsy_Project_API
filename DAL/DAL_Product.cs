using NHibernate;
using NHibernate.Linq;
using Shopsy_Project.Common;
using Shopsy_Project.Extensions;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace Shopsy_Project.DAL
{
    public class DAL_Product : IDAL_Product
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HttpContextAccessor _httpContextAccessor;

        public DAL_Product()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        public PagedResult<Products> GetAllProducts(PaginationFilter filter, int userId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var query = session.Query<Products>();

                // Apply search filter
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(p =>
                        (p.Product_Name != null && p.Product_Name.Contains(filter.SearchTerm)) ||
                        (p.Color != null && p.Color.Contains(filter.SearchTerm)));
                }

                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                if (filter != null && !string.IsNullOrEmpty(filter.SortBy))
                {
                    filter.SortBy = textInfo.ToTitleCase(filter.SortBy);


                    if (!string.IsNullOrEmpty(filter.SortBy))
                    {
                        query = filter.IsAscending
                            ? query.OrderByDynamic(filter.SortBy, true)
                            : query.OrderByDynamic(filter.SortBy, false);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.Product_Id);
                    }
                }
                var totalCount = query.Count();

                var paginatedItems = query
                    .Skip(filter != null ? filter.Skip : 0)
                    .Take(filter != null ? filter.PageSize : 10)
                    .ToList();

                var productIds = paginatedItems.Select(p => p.Product_Id).ToList();
                var cartCounts = session.Query<Cart>()
                    .Where(c => c.userId == userId && productIds.Contains(c.product_Id))
                    .GroupBy(c => c.product_Id)
                    .Select(g => new { ProductId = g.Key, Count = g.Sum(c => c.Quantity) })
                    .ToDictionary(x => x.ProductId, x => x.Count);

                foreach (var product in paginatedItems)
                {
                    product.Cartcount = cartCounts.ContainsKey(product.Product_Id) ? cartCounts[product.Product_Id] : 0;
                }

                var Isfavourites = session.Query<WishItem>()
                    .Where(c => c.userId == userId).ToList();

                //update product where productid & userid same for updating Isfavourite property.
                var favouriteItems = session.Query<WishItem>()
                .Where(c => c.userId == userId)
                .Select(w => w.productid) // Assuming WishItem has a ProductId property
                .ToHashSet(); // Use HashSet for quick lookup

                // Update IsFavourite property for products
                foreach (var product in paginatedItems)
                {
                    product.Isfavourite = favouriteItems.Contains(product.Product_Id);
                }

                return new PagedResult<Products>
                {
                    Items = paginatedItems,
                    TotalCount = totalCount,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
            }
        }


        // Get product by ID
        public Products GetProductById(int productId, int userId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                Products Product = session.Get<Products>(productId) ?? new Products();

                Product.Cartcount = session.Query<Cart>().Where(x=>x.userId == userId && x.product_Id == Product.Product_Id).FirstOrDefault()?.Quantity;

                Product.Isfavourite = session.Query<WishItem>()
                    .Where(c => c.userId == userId && c.productid == Product.Product_Id).FirstOrDefault()?.Isfavourite;

                Product.Categories = session.Query<Categories>().Where(x=>x.category_Id == Product.Category_Id).FirstOrDefault();
                Product.Brands = session.Query<Brands>().Where(x => x.brand_Id == Product.Brand_Id).FirstOrDefault();

                return Product;
            }
        }

        //Get All Categories
        public List<Categories> GetAllCategories()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<Categories>().ToList();
            }
        }

        //Get All Brands
        public List<Brands> GetAllBrands()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<Brands>().ToList();
            }
        }

        // Add a product
        public void AddProduct(Products product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            using (var session = _sessionFactory.OpenSession())
            {
                if (!session.Query<Brands>().Any(x => x.brand_Id == product.Brand_Id))
                    throw new ArgumentException("Invalid Brand ID.");

                if (!session.Query<Categories>().Any(x => x.category_Id == product.Category_Id))
                    throw new ArgumentException("Invalid Category ID.");

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(product);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("An error occurred while adding the product.", ex);
                    }
                }
            }
        }

        // Update a product
        public void UpdateProduct(Products product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var mergedProduct = session.Merge(product); // Handles detached objects
                    session.Update(mergedProduct);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("An error occurred while updating the product.", ex);
                }
            }
        }

        // Delete a product
        public void DeleteProduct(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("Invalid product ID.");

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var product = session.Get<Products>(productId);
                    if (product == null)
                        throw new ArgumentException($"No product found with ID {productId}.");

                    session.Delete(product);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("An error occurred while deleting the product.", ex);
                }
            }
        }
    }
}
