using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.DTO
{
    public class MyAppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; }

        public int BarberId { get; set; }
        public string BarberName { get; set; } = null!;

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
