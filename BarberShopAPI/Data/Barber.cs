namespace BarberShopAPI.Data
{
    public class Barber : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
