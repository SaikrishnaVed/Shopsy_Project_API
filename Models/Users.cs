using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopsy_Project.Models
{
    [Table("Users")] // Specifies the table name in the database
    public class Users
    {
        [Key] // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremented
        public virtual int Id { get; set; }

        //[Required(ErrorMessage = "Name is required")]
        public virtual string? Name { get; set; }

        public virtual string? Description { get; set; }

        //[Required(ErrorMessage = "Date of Birth is required")] // Makes DateOfBirth mandatory
        [DataType(DataType.Date)] // Specifies date format
        public virtual DateTime? DateOfBirth { get; set; }

        //[Required(ErrorMessage = "Email is required")] // Makes Email mandatory
        [EmailAddress(ErrorMessage = "Invalid Email Address")] // Validates email format
        public virtual string? Email { get; set; }

        //[Required(ErrorMessage = "Phone is required")] // Makes Phone mandatory
        //[Phone(ErrorMessage = "Invalid Phone Number")] // Validates phone format
        public virtual string? Phone { get; set; }

        //[Required(ErrorMessage = "Gender is required")] // Makes Gender mandatory
        public virtual string? Gender { get; set; }

        //[Required(ErrorMessage = "Password is required")] // Makes Password mandatory
        //[StringLength(10, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 10 characters")]
        public virtual string? Password { get; set; }

        public virtual bool? isActive { get; set; }
    }
}