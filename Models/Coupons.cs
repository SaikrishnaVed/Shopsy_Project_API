using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shopsy_Project.Models
{
    [Table("coupons")]
    public class Coupons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual string code { get; set; }
        public virtual string discount_type { get; set; }
        public virtual decimal discount_value { get; set; }
        public virtual DateTime start_date { get; set; }
        public virtual DateTime end_date { get; set; }
        public virtual int usage_limit { get; set; }
        public virtual string status { get; set; }
    }
}