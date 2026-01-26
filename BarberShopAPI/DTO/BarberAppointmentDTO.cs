using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.DTO
{
    public class BarberAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;

        public string ServiceName { get; set; } = null!;
        public int DurationMinutes { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}
