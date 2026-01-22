namespace BarberShopAPI.DTO
{
    public class AvailabilityDTO
    {
        public int AppointmentId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ServiceId { get; set; }
    }
}
