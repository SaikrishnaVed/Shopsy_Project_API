using Shopsy_Project.DAL;
using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IDAL_Brand
    {
        List<Brands> GetAllBrands();
        void AddNewBrand(Brands brand);
    }
}