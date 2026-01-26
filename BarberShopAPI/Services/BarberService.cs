using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BarberShopAPI.Services
{
    public class BarberService : IBarberService
    {
        private readonly AppDbContext _context;

        public BarberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BarberAppointmentDTO>> GetMyAppointmentsAsync(int userId, DateTime? date = null)
        {
            // βρίσκω barber profile από logged-in userId
            var barber = await _context.Barbers
                .FirstOrDefaultAsync(b => b.UserId == userId && !b.IsDeleted);

            if (barber == null)
                throw new NotFoundException("BARBER", "Barber profile not found for this user.");

            // query για appointments του barber
            var query = _context.Appointments
                .AsNoTracking()
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Where(a => !a.IsDeleted && a.BarberId == barber.Id);

            // φίλτρο ημερομηνίας
            if (date.HasValue)
            {
                var d = date.Value.Date;
                query = query.Where(a => a.AppointmentDateTime.Date == d);
            }

            return await query
                .OrderBy(a => a.AppointmentDateTime)
                .Select(a => new BarberAppointmentDTO
                {
                    AppointmentId = a.Id,
                    Start = a.AppointmentDateTime,
                    End = a.AppointmentDateTime.AddMinutes(a.Service.DurationMinutes),

                    CustomerName = a.Customer.FirstName + " " + a.Customer.LastName,
                    CustomerPhone = a.Customer.PhoneNumber,

                    ServiceName = a.Service.Name,
                    DurationMinutes = a.Service.DurationMinutes,

                    Status = a.Status
                })
                .ToListAsync();
        }
    }
}
