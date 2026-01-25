using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;

namespace BarberShopAPI.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAsync(int userId, CreateAppointmentDTO createAppointmentDTO);
        Task<List<AvailabilityDTO>> GetBookedSlotsAsync(int barberId, DateTime date);
        Task<AppointmentDetailsDTO> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, AppointmentStatus status);
        Task<List<MyAppointmentDTO>> GetMyAppointmentsAsync(int userId);
    }
}
