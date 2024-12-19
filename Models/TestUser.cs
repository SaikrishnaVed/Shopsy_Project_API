using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    [Table("TestUser")]
    public class TestUser
    {
        [Key]
        public virtual int User_Id { get; set; }

        //[Column(TypeName = "nvarchar(50)")] // Specifies nvarchar(50) in the database
        public virtual string? FirstName { get; set; }

        //[Column(TypeName = "nvarchar(50)")]
        public virtual string? SecondName { get; set; }
    }
}
