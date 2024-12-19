using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    public class AuthUserTokens
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual string? Token { get; set; }
        public virtual DateTime Expiration { get; set; }
        public virtual string? RefreshToken { get; set; }
        public virtual DateTime RefreshTokenExpiration { get; set; }
    }
}