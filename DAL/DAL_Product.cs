using NHibernate;
using NHibernate.Linq;
using Shopsy_Project.Common;
using Shopsy_Project.Extensions;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopsy_Project.DAL
{
    public class DAL_Product : IDAL_Product
    {
        private readonly ISessionFactory _sessionFactory;

        public DAL_Product()
        {
            var config = ConfigurationManager.SetConfiguration();
            _sessionFactory = config.BuildSessionFactory();
        }

        // Get all products with pagination and filtering
        public PagedResult<Products> GetAllProducts(PaginationFilter filter)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var query = session.Query<Products>();

                // Apply search filter
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(p =>
                        p.Product_Name != null && p.Product_Name.Contains(filter.SearchTerm) ||
                        p.Color != null &&  p.Color.Contains(filter.SearchTerm));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(filter.SortBy))
                {
                    query = filter.IsAscending
                        ? query.OrderByDynamic(filter.SortBy, true)
                        : query.OrderByDynamic(filter.SortBy, false);
                }
                else
                {
                    query = query.OrderBy(p => p.Product_Id); // Default sorting by Product_Id
                }

                // Get total count before pagination
                var totalCount = query.Count();

                // Apply pagination
                var paginatedItems = query
                    .Skip(filter.Skip)
                    .Take(filter.PageSize)
                    .ToList();

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
        public Products GetProductById(int productId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<Products>(productId) ?? new Products();
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
