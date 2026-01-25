using System.ComponentModel.DataAnnotations;

namespace BarberShopAPI.DTO
{
    public class UserLoginDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 2,
        ErrorMessage = "Username must be between 2 and 30 characters.")]
        public string Username { get; set; } = null!;

        [Required]
        [RegularExpression(
            @"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W)^.{8,}$",
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character")]
        public string Password { get; set; } = null!;

        public bool KeepLoggedIn { get; set; }
    }
}
