using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    public class AuthUsers
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented
        public virtual int Id { get; set; }
        public virtual string? UserName { get; set; }
        public virtual string? Email { get; set; }
        public virtual string? PasswordHash { get; set; }
        public virtual string? Role { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
