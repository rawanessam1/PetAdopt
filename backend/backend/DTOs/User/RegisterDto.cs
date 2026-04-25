using backend.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.User
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
