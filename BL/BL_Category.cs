using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.BL
{
    public class BL_Category : IBL_Category
    {
        private readonly IDAL_Category _dAL_Category;

        public BL_Category(IDAL_Category dAL_Category)
        {
            _dAL_Category = dAL_Category;
        }

        public void AddNewCategory(Categories category)
        {
            _dAL_Category.AddNewCategory(category);
        }

        public List<Categories> GetAllCategories()
        {
            return _dAL_Category.GetAllCategories();
        }
    }
}
