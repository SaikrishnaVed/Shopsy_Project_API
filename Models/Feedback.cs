using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Shopsy_Project.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual int rating { get; set; }
        public virtual string comments { get; set; }
        public virtual int userId { get; set; }
        public virtual int product_Id { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [NotMapped]
        public virtual string username { get; set; }
    }
}
