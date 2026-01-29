using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.DTO
{
    public class BarberAppointmentItemDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; }

        public string ServiceName { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
    }
}
