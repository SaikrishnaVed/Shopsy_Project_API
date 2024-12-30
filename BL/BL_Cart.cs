using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.BL
{
    public class BL_Cart : IBL_Cart
    {
        private readonly IDAL_Cart _dalCart;

        public BL_Cart(IDAL_Cart dalCart)
        {
            _dalCart = dalCart;
        }

        public List<Cart> GetAllCartItems(int userId)
        {
            return _dalCart.GetAllCartItems(userId);
        }

        public void AddCartItems(AddCartRequest cart)
        {
            _dalCart.AddCartItems(cart);
        }

        public void UpdateCartItems(Cart cart)
        {
            _dalCart.UpdateCartItems(cart);
        }

        public void DeleteCartItems(int id)
        {
            _dalCart.DeleteCartItems(id);
        }

        public Cart GetCartById(int Id)
        {
            return _dalCart.GetCartById(Id);
        }
    }
}