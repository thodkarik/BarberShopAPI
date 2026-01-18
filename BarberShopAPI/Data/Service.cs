namespace BarberShopAPI.Data
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
    }
}
