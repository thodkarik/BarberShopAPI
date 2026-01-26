using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.DTO
{
    public class ReceptionistAppointmentDTO
    {
        public int AppointmentId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string BarberName { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;

        public string ServiceName { get; set; } = null!;
        public AppointmentStatus Status { get; set; }
    }
}
