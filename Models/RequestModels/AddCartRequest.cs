using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopsy_Project.Models.RequestModels
{
    public class AddCartRequest
    {
        public int userId { get; set; }
        public int product_Id { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}