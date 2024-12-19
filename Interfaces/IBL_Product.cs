using Shopsy_Project.Common;
using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_Product
    {
        PagedResult<Products> GetAllProducts(PaginationFilter filter);
        Products GetProductById(int productId);
        void AddProduct(Products product);
        void UpdateProduct(Products product);
        void DeleteProduct(int productId);
    }
}
