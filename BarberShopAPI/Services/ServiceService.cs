using BarberShopAPI.Data;
using BarberShopAPI.Exceptions;
using BarberShopAPI.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BarberShopAPI.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Service> _serviceRepository;

        public ServiceService(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public Task<List<Service>> GetAllAsync()
            => _serviceRepository.GetAllAsync(s => !s.IsDeleted);

        public async Task<Service> GetByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            if (service == null || service.IsDeleted)
                throw new NotFoundException("SERVICE", $"Service with id {id} was not found");

            return service;
        }

        public async Task<Service> CreateAsync(Service service)
        {
            if (string.IsNullOrWhiteSpace(service.Name))
                throw new BadRequestException("SERVICE", "Service name is required");

            await _serviceRepository.AddAsync(service);
            await _serviceRepository.SaveChangesAsync();

            return service;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await GetByIdAsync(id);

            existing.IsDeleted = true;
            existing.DeletedAt = DateTime.UtcNow;

            _serviceRepository.Update(existing);
            await _serviceRepository.SaveChangesAsync();
        }

        public async Task<Service> UpdateAsync(int id, Service updatedService)
        {
            if (id != updatedService.Id)
                throw new BadRequestException("SERVICE", "Id mismatch");

            var existing = await GetByIdAsync(id);

            existing.Name = updatedService.Name;
            existing.DurationMinutes = updatedService.DurationMinutes;
            existing.Price = updatedService.Price;
            existing.UpdatedAt = DateTime.UtcNow;

            _serviceRepository.Update(existing);
            await _serviceRepository.SaveChangesAsync();

            return existing;
        }
    }
}
