using BarberShopAPI.Data;
using BarberShopAPI.DTO;

namespace BarberShopAPI.Services
{
    public interface IServiceService
    {
        Task<List<ServiceResponseDTO>> GetAllAsync();
        Task<ServiceResponseDTO> GetByIdAsync(int id);
        Task<ServiceResponseDTO> CreateAsync(CreateServiceDTO dto);
        Task UpdateAsync(int id, UpdateServiceDTO dto);
        Task DeleteAsync(int id);
    }
}
