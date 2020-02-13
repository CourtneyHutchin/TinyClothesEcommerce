using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    /// <summary>
    /// A single user account
    /// </summary>
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        /// <summary>
        /// First and last name
        /// </summary>
        [Required]
        [StringLength(60)]
        public string FullName { get; set; }
        [Required]
        [StringLength(20)]
        public string Username { get; set; }
        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(60)]
        public string FullName { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))] // This is how we can make sure its the same password
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}