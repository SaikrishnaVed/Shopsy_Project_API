using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Category
    {
        List<Categories> GetAllCategories();
        void AddNewCategory(Categories category);
    }
}