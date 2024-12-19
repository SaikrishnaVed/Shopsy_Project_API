using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopsy_Project.Models
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented
        public virtual int category_Id { get; set; }
        [Required(ErrorMessage = "category Name is required")]
        public virtual string? category_Name { get; set; }
    }
}
