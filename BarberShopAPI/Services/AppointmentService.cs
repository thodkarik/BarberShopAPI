using AutoMapper;
using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Exceptions;
using BarberShopAPI.Repositories;
using Microsoft.EntityFrameworkCore;


namespace BarberShopAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Service> _serviceRepository;
        private readonly AppDbContext _context;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Barber> _barberRepository;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            IRepository<Service> serviceRepository, 
            AppDbContext context, 
            IRepository<Customer> customerRepository,
            IMapper mapper,
            IRepository<Barber> barberRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _context = context;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _barberRepository = barberRepository;
        }

        public async Task<AppointmentCreatedDTO> CreateAsync(int userId, CreateAppointmentDTO createAppointmentDTO)
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

            var customer = await _customerRepository.GetAllAsync(c => c.UserId == userId && !c.IsDeleted);
            var customerEntity = customer.FirstOrDefault();

            if (customerEntity == null)
            {
                throw new NotFoundException("CUSTOMER", "Customer profile not found for this user.");
            }

            var appointment = new Appointment
            {
                CustomerId = customerEntity.Id,
                BarberId = createAppointmentDTO.BarberId,
                ServiceId = createAppointmentDTO.ServiceId,
                AppointmentDateTime = start,
                Status = AppointmentStatus.Scheduled,
                IsDeleted = false
            };

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return _mapper.Map<AppointmentCreatedDTO>(appointment);
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

        public async Task<AppointmentDetailsDTO> GetByIdAsync(int id, int userId)
        {
            var customer = (await _customerRepository.GetAllAsync(c => c.UserId == userId && !c.IsDeleted))
                .FirstOrDefault();

            if (customer == null)
                throw new NotFoundException("CUSTOMER", "Customer profile not found for this user.");

            var appointment = await _context.Appointments
                .Include(a => a.Barber)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            if (appointment == null)
                throw new NotFoundException("APPOINTMENT", "Appointment not found.");

            if (appointment.CustomerId != customer.Id)
                throw new ForbiddenException("APPOINTMENT", "You cannot access this appointment."); // αν δεν έχεις, κάνε Conflict/BadRequest προσωρινά

            var dto = _mapper.Map<AppointmentDetailsDTO>(appointment);
            dto.DurationMinutes = appointment.Service.DurationMinutes;
            dto.Price = appointment.Service.Price;

            return dto;
        }

        public async Task UpdateStatusAsync(int id, int userId, AppointmentStatus newStatus)
        {
            var customer = (await _customerRepository.GetAllAsync(c => c.UserId == userId && !c.IsDeleted))
                .FirstOrDefault();

            if (customer == null)
                throw new NotFoundException("CUSTOMER", "Customer profile not found for this user.");

            var appt = await _appointmentRepository.GetByIdAsync(id);

            if (appt == null || appt.IsDeleted)
                throw new NotFoundException("APPOINTMENT", "Appointment not found.");

            if (appt.CustomerId != customer.Id)
                throw new ForbiddenException("APPOINTMENT", "You cannot update this appointment.");

            // οι κανόνες που έχεις ήδη
            if (appt.Status != AppointmentStatus.Scheduled)
                throw new ConflictException("APPOINTMENT", "Only scheduled appointments can change status");

            if (newStatus != AppointmentStatus.Completed && newStatus != AppointmentStatus.Canceled)
                throw new BadRequestException("APPOINTMENT", "Invalid status transition");

            appt.Status = newStatus;
            appt.UpdatedAt = DateTime.UtcNow;

            _appointmentRepository.Update(appt);
            await _appointmentRepository.SaveChangesAsync();
        }

        private async Task<Appointment> GetEntityByIdAsync(int id)
        {
            var appt = await _appointmentRepository.GetByIdAsync(id);
            if (appt == null || appt.IsDeleted)
                throw new NotFoundException("APPOINTMENT", $"Appointment with id {id} was not found");

            return appt;
        }

        public async Task<List<MyAppointmentDTO>> GetMyAppointmentsAsync(int userId)
        {
            // Βρίσκουμε Customer profile με βάση το UserId (1-1 σχέση)
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);

            if (customer == null)
                throw new NotFoundException("CUSTOMER", "Customer profile not found for this user");

            // Φέρνουμε όλα τα appointments του customer
            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Barber)
                .Include(a => a.Service)
                .Where(a => a.CustomerId == customer.Id && !a.IsDeleted)
                .OrderByDescending(a => a.AppointmentDateTime)
                .ToListAsync();

            // Map σε DTO για καθαρό response χωρίς cycles
            return appointments.Select(a => new MyAppointmentDTO
            {
                Id = a.Id,
                AppointmentDateTime = a.AppointmentDateTime,
                Status = a.Status,
                BarberId = a.BarberId,
                BarberName = $"{a.Barber.FirstName} {a.Barber.LastName}",
                ServiceId = a.ServiceId,
                ServiceName = a.Service.Name,
                DurationMinutes = a.Service.DurationMinutes,
                Price = a.Service.Price
            }).ToList();
        }

        public async Task<List<AvailabilitySlotDTO>> GetAvailabilityAsync(DateOnly date, int barberId, int serviceId)
        {
            var day = date.ToDateTime(TimeOnly.MinValue);
            if (day.DayOfWeek == DayOfWeek.Sunday)
                return new List<AvailabilitySlotDTO>();

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null || service.IsDeleted)
                throw new NotFoundException("SERVICE", "Service not found.");

            var duration = service.DurationMinutes;
            if (duration != 30 && duration != 60)
                throw new BadRequestException("SERVICE", "Service duration must be 30 or 60 minutes.");

            var barber = await _barberRepository.GetByIdAsync(barberId);
            if (barber == null || barber.IsDeleted)
                throw new NotFoundException("BARBER", "Barber not found.");

            var open = date.ToDateTime(new TimeOnly(10, 0));
            var close = date.ToDateTime(new TimeOnly(20, 0));

            var existingAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.BarberId == barberId &&
                !a.IsDeleted &&
                a.Status == AppointmentStatus.Scheduled &&
                a.AppointmentDateTime >= open &&
                a.AppointmentDateTime < close);

            var existingServiceIds = existingAppointments.Select(a => a.ServiceId).Distinct().ToList();
            var existingServices = await _serviceRepository.GetAllAsync(s =>
                existingServiceIds.Contains(s.Id) && !s.IsDeleted);

            var durationByServiceId = existingServices.ToDictionary(s => s.Id, s => s.DurationMinutes);

            var slots = new List<AvailabilitySlotDTO>();
            for (var start = open; start < close; start = start.AddMinutes(30))
            {
                var end = start.AddMinutes(duration);

                if (end > close) continue;

                var overlaps = false;
                foreach (var appt in existingAppointments)
                {
                    if (!durationByServiceId.TryGetValue(appt.ServiceId, out var apptDuration))
                        apptDuration = 30; 

                    var apptStart = appt.AppointmentDateTime;
                    var apptEnd = apptStart.AddMinutes(apptDuration);

                    if (start < apptEnd && end > apptStart)
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (!overlaps)
                {
                    slots.Add(new AvailabilitySlotDTO { Start = start, End = end });
                }
            }

            return slots;
        }
    }
}
