using BarberShopAPI.Data;
using BarberShopAPI.DTO;

namespace BarberShopAPI.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAsync(CreateAppointmentDTO createAppointmentDTO);
        Task<List<AvailabilityDTO>> GetBookedSlotsAsync(int barberId, DateTime date);
    }
}
