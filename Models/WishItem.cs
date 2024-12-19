using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    public class WishItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented
        public virtual int Id { get; set; }
        public virtual bool Isfavourite { get; set; }
        public virtual int userId { get; set; }
        public virtual int productId { get; set; }
    }
}