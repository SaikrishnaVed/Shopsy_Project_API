using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    public class Login
    {
        public string? LoginName { get; set; }
        public string? Password { get; set; }
    }
}
