using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_WishItem
    {
        public List<WishItem> WishItemList();
        void AddWishItem(WishItem wishItem);
        void DeleteWishItem(int id);
    }
}