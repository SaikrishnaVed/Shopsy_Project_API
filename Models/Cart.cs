using Shopsy_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopsy_Project.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Cart_Id { get; set; }
        [Required(ErrorMessage = "user Id is required")]
        public virtual int userId { get; set; }
        [Required(ErrorMessage = "product Id is required")]
        public virtual int product_Id { get; set; }
        [Required(ErrorMessage = "brand name is required")]
        public virtual int? Quantity { get; set; }

        public virtual DateTime? DateCreated { get; set; }
        [NotMapped]
        public virtual Users? user { get; set; }
        [NotMapped]
        public virtual List<Products>? products { get; set; }
    }
}