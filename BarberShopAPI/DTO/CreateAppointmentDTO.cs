namespace BarberShopAPI.DTO
{
    public class CreateAppointmentDTO
    {
        public int CustomerId { get; set; }
        public int BarberId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
