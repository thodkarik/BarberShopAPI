using BarberShopAPI.DTO;

namespace BarberShopAPI.Services
{
    public interface IBarberService
    {
        Task<List<BarberAppointmentDTO>> GetMyAppointmentsAsync(int userId, DateTime? date = null);
    }
}
