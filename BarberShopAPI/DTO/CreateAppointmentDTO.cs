namespace BarberShopAPI.DTO
{
    public class CreateAppointmentDTO
    {
        public int BarberId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
