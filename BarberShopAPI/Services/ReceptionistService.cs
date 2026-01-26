using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BarberShopAPI.Services
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly AppDbContext _context;

        public ReceptionistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReceptionistAppointmentDTO>> GetAppointmentsAsync(DateTime? date = null)
        {
            var query = _context.Appointments
                .AsNoTracking()
                .Include(a => a.Barber)
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Where(a => !a.IsDeleted);

            if (date.HasValue)
            {
                var d = date.Value.Date;
                query = query.Where(a => a.AppointmentDateTime.Date == d);
            }

            return await query
                .OrderBy(a => a.AppointmentDateTime)
                .Select(a => new ReceptionistAppointmentDTO
                {
                    AppointmentId = a.Id,
                    Start = a.AppointmentDateTime,
                    End = a.AppointmentDateTime.AddMinutes(a.Service.DurationMinutes),

                    BarberName = a.Barber.FirstName + " " + a.Barber.LastName,
                    CustomerName = a.Customer.FirstName + " " + a.Customer.LastName,
                    CustomerPhone = a.Customer.PhoneNumber,

                    ServiceName = a.Service.Name,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task UpdateAppointmentStatusAsync(int appointmentId, AppointmentStatus newStatus)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId && !a.IsDeleted);

            if (appointment == null)
                throw new NotFoundException("APPOINTMENT", "Appointment not found.");

            if (newStatus != AppointmentStatus.Completed &&
                newStatus != AppointmentStatus.Canceled &&
                newStatus != AppointmentStatus.NoShow)
            {
                throw new BadRequestException("APPOINTMENT", "Invalid status.");
            }

            appointment.Status = newStatus;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
