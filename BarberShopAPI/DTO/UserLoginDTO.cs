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
        [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Password must be between 2 and 30 characters.")]
        public string Password { get; set; } = null!;

        public bool KeepLoggedIn { get; set; }
    }
}
