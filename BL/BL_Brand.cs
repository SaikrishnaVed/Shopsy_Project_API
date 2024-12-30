using Shopsy_Project.DAL;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;

namespace Shopsy_Project.BL
{
    public class BL_Brand : IBL_Brand
    {
        private readonly IDAL_Brand _dAL_Brand;

        public BL_Brand(IDAL_Brand dAL_Brand)
        {
            _dAL_Brand = dAL_Brand;
        }

        public void AddNewBrand(Brands brand)
        {
            _dAL_Brand.AddNewBrand(brand);
        }

        public List<Brands> GetAllBrands()
        {
            return _dAL_Brand.GetAllBrands();
        }
    }
}