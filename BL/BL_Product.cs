using Shopsy_Project.Common;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.BL
{
    public class BL_Product : IBL_Product
    {
        private readonly IDAL_Product _dalProduct;

        public BL_Product(IDAL_Product dalProduct)
        {
            _dalProduct = dalProduct;
        }

        public PagedResult<Products> GetAllProducts(PaginationFilter filter)
        {
            return _dalProduct.GetAllProducts(filter);
        }

        public Products GetProductById(int productId)
        {
            return _dalProduct.GetProductById(productId);
        }

        public void AddProduct(Products product)
        {
            _dalProduct.AddProduct(product);
        }

        public void UpdateProduct(Products product)
        {
            _dalProduct.UpdateProduct(product);
        }

        public void DeleteProduct(int productId)
        {
            _dalProduct.DeleteProduct(productId);
        }
    }
}
