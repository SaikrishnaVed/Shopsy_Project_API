using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_Category
    {
        List<Categories> GetAllCategories();
        void AddNewCategory(Categories category);
    }
}