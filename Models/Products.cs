using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented
        public virtual int Product_Id { get; set; }
        //[Required(ErrorMessage = "ProductName is required")] // Makes DateOfBirth mandatory
        public virtual string? Product_Name { get; set; }
        public virtual int Brand_Id { get; set; }
        public virtual int Category_Id { get; set; }
        //[Required(ErrorMessage = "ModelYear is required")] // Makes DateOfBirth mandatory
        public virtual int Model_Year { get; set; }
        //[Required(ErrorMessage = "ListPrice is required")] // Makes DateOfBirth mandatory
        public virtual decimal List_Price { get; set; }
        //[Required(ErrorMessage = "Quantity is required")] // Makes DateOfBirth mandatory
        public virtual int Quantity { get; set; }
        //[Required(ErrorMessage = "Color is required")] // Makes DateOfBirth mandatory
        public virtual string? Color { get; set; }
    }
}