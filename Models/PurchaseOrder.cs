using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopsy_Project.Models
{
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual int Product_Id { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int userId { get; set; }
        public virtual decimal price { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}