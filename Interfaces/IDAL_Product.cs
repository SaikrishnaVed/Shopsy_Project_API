using Shopsy_Project.Common;
using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Product
    {
        PagedResult<Products> GetAllProducts(PaginationFilter filter, int userid);
        Products GetProductById(int productId, int userId);
        void AddProduct(Products product);
        void UpdateProduct(Products product);
        void DeleteProduct(int productId);
        List<Categories> GetAllCategories();
        List<Brands> GetAllBrands();
    }
}
