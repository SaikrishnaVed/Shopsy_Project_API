using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopsy_Project.Models
{
    public class Brands
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented
        public virtual int brand_Id { get; set; }
        [Required(ErrorMessage = "brand name is required")]
        public virtual string? brand_Name { get; set; }
    }
}
