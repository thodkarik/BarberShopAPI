using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Exceptions;
using BarberShopAPI.Repositories;

namespace BarberShopAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Service> _serviceRepository;

        public AppointmentService(IRepository<Appointment> appointmentRepository, IRepository<Service> serviceRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<Appointment> CreateAsync(CreateAppointmentDTO createAppointmentDTO)
        {
            var start = createAppointmentDTO.AppointmentDateTime;

            if (start < DateTime.Now)
            {
                throw new BadRequestException("APPOINTMENT", "Appointment date and time must be in the future.");
            }

            if (start.Minute != 0 && start.Minute != 30)
            {
                throw new BadRequestException("APPOINTMENT", "Appointment time must be on the hour or half hour.");
            }

            if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new BadRequestException("APPOINTMENT", "Barbershop is closed on Sundays.");
            }

            var service = await _serviceRepository.GetByIdAsync(createAppointmentDTO.ServiceId);
            if (service == null || service.IsDeleted)
            {
                throw new NotFoundException("SERVICE", "Service not found.");
            }

            if (service.DurationMinutes != 30 && service.DurationMinutes != 60)
            {
                throw new BadRequestException("SERVICE", "Service duration must be either 30 or 60 minutes.");
            }

            var end = start.AddMinutes(service.DurationMinutes);

            var open = start.Date.AddHours(10); // 10:00 AM
            var close = start.Date.AddHours(20); // 20:00 PM

            if (start < open || end > close)
            {
                throw new BadRequestException("APPOINTMENT", "Appointment time is outside of business hours (10:00 AM - 8:00 PM).");
            }

            var barberAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.BarberId == createAppointmentDTO.BarberId &&
                a.Status == AppointmentStatus.Scheduled &&
               !a.IsDeleted);

            var serviceIds = barberAppointments.Select(a => a.ServiceId).Distinct().ToList();
            var services = await _serviceRepository.GetAllAsync(s => serviceIds.Contains(s.Id) && !s.IsDeleted);
            var durationByServiceId = services.ToDictionary(s => s.Id, s => s.DurationMinutes);

            foreach (var existing in barberAppointments)
            {
                if (!durationByServiceId.TryGetValue(existing.ServiceId, out var existingDuration))
                    continue;

                var existingStart = existing.AppointmentDateTime;
                var existingEnd = existingStart.AddMinutes(existingDuration);

                bool overlaps = start < existingEnd && end > existingStart;

                if (overlaps)
                    throw new ConflictException("APPOINTMENT", "Selected time slot is not available");
            }

            var appointment = new Appointment
            {
                CustomerId = createAppointmentDTO.CustomerId,
                BarberId = createAppointmentDTO.BarberId,
                ServiceId = createAppointmentDTO.ServiceId,
                AppointmentDateTime = start,
                Status = AppointmentStatus.Scheduled,
                IsDeleted = false
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return appointment;
        }

        public async Task<List<AvailabilityDTO>> GetBookedSlotsAsync(int barberId, DateTime date)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            var appointments = await _appointmentRepository.GetAllAsync(a =>
                a.BarberId == barberId &&
                a.Status == AppointmentStatus.Scheduled &&
                !a.IsDeleted &&
                a.AppointmentDateTime >= dayStart &&
                a.AppointmentDateTime < dayEnd);

            if (appointments.Count == 0)
                return new List<AvailabilityDTO>();

            var serviceIds = appointments.Select(a => a.ServiceId).Distinct().ToList();
            var services = await _serviceRepository.GetAllAsync(s => serviceIds.Contains(s.Id) && !s.IsDeleted);
            var durationByServiceId = services.ToDictionary(s => s.Id, s => s.DurationMinutes);

            var result = new List<AvailabilityDTO>();

            foreach (var appt in appointments)
            {
                if (!durationByServiceId.TryGetValue(appt.ServiceId, out var duration))
                    continue;

                var start = appt.AppointmentDateTime;
                var end = start.AddMinutes(duration);

                result.Add(new AvailabilityDTO
                {
                    AppointmentId = appt.Id,
                    Start = start,
                    End = end,
                    ServiceId = appt.ServiceId
                });
            }

            return result.OrderBy(x => x.Start).ToList();
        }

    }
}
