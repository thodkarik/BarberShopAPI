using BarberShopAPI.Core.Enums;
using BarberShopAPI.DTO;

namespace BarberShopAPI.Services
{
    public interface IReceptionistService
    {
        Task<List<ReceptionistAppointmentDTO>> GetAppointmentsAsync(DateTime? date = null);
        Task UpdateAppointmentStatusAsync(int appointmentId, AppointmentStatus newStatus);
    }
}
