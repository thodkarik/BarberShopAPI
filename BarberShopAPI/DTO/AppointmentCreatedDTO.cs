using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.DTO
{
    public class AppointmentCreatedDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
