using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;

namespace BarberShopAPI.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentCreatedDTO> CreateAsync(int userId, CreateAppointmentDTO dto);
        Task<List<AvailabilityDTO>> GetBookedSlotsAsync(int barberId, DateTime date);
        Task<AppointmentDetailsDTO> GetByIdAsync(int id, int userId);
        Task UpdateStatusAsync(int id, int userId, AppointmentStatus newStatus);
        Task<List<MyAppointmentDTO>> GetMyAppointmentsAsync(int userId);
    }
}
