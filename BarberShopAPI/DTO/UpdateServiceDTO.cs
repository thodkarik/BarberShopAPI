using System.ComponentModel.DataAnnotations;

namespace BarberShopAPI.DTO
{
    public class UpdateServiceDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Range(30, 60)]
        public int DurationMinutes { get; set; }

        public decimal Price { get; set; }
    }
}
