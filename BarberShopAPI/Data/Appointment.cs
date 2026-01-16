using BarberShopAPI.Core.Enums;

namespace BarberShopAPI.Data
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int BarberId { get; set; }
        public Barber Barber { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
