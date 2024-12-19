using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.BL
{
    public class BL_WishItem : IBL_WishItem
    {
        private readonly IDAL_WishItem _dalWishItem;

        public BL_WishItem(IDAL_WishItem dalWishItem)
        {
            _dalWishItem = dalWishItem;
        }

        public void AddWishItem(WishItem wishItem)
        {
            _dalWishItem.AddWishItem(wishItem);
        }

        public void DeleteWishItem(int id)
        {
            _dalWishItem.DeleteWishItem(id);
        }

        public List<WishItem> WishItemList()
        {
            return _dalWishItem.WishItemList();
        }
    }
}
