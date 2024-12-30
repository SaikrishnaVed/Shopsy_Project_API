using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Product_Id { get; set; }
        public virtual string? Product_Name { get; set; }
        public virtual int Brand_Id { get; set; }
        public virtual int Category_Id { get; set; }
        public virtual int Model_Year { get; set; }
        public virtual decimal List_Price { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string? Color { get; set; }
        public virtual string? imagePath { get; set; }
        [NotMapped]
        public virtual int? Cartcount { get; set; }
        [NotMapped]
        public virtual bool? Isfavourite { get; set; }
        [NotMapped]
        public virtual Categories? Categories { get; set; }
        [NotMapped]
        public virtual Brands? Brands { get; set; }
    }
}