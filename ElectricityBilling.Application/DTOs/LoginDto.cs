using System.ComponentModel.DataAnnotations;

namespace ElectricityBilling.Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(200, ErrorMessage = "Password cannot exceed 200 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}