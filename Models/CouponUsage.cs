using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Shopsy_Project.Models
{
    public class CouponUsage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual int Coupon_Id { get; set; }
        public virtual int User_Id { get; set; }
        public virtual int Order_Id { get; set; }
        public virtual DateTime Usage_Date { get; set; }
    }
}