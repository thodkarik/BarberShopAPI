using BarberShopAPI.Data;

namespace BarberShopAPI.Services
{
    public interface IServiceService
    {
        Task<List<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(int id);
        Task<Service> CreateAsync(Service service);
        Task<Service> UpdateAsync(int id, Service updatedService);
        Task DeleteAsync(int id);
    }
}
