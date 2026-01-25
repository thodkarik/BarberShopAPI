using System.ComponentModel.DataAnnotations;

namespace BarberShopAPI.DTO
{
    public class CustomerSignupDTO
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 30 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Firstname must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname must be between 2 and 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [RegularExpression(
            @"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W)^.{8,}$",
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character")]
        public string Password { get; set; } = null!;
    }
}
