using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.DTO
{
    public class AppointmentDetailsDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; } = null!;

        public int BarberId { get; set; }
        public string BarberFullName { get; set; } = null!;

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
