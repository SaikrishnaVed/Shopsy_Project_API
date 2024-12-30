using Shopsy_Project.Models;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_Brand
    {
        List<Brands> GetAllBrands();
        void AddNewBrand(Brands brand);
    }
}