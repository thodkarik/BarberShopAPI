namespace BarberShopAPI.Data
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
