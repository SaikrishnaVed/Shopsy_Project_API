using Shopsy_Project.Common;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Cart
    {
        List<Cart> GetAllCartItems();
        Cart GetCartById(int Id);
        void AddCartItems(AddCartRequest cart);
        void UpdateCartItems(Cart cart);
        void DeleteCartItems(int cartId);
    }
}
