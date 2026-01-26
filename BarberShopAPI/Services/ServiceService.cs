using AutoMapper;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Exceptions;
using BarberShopAPI.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BarberShopAPI.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Service> _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IRepository<Service> serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceResponseDTO>> GetAllAsync()
        {
            var services = await _serviceRepository.GetAllAsync(s => !s.IsDeleted);
            return _mapper.Map<List<ServiceResponseDTO>>(services);
        }

        public async Task<ServiceResponseDTO> GetByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            if (service == null || service.IsDeleted)
                throw new NotFoundException("SERVICE", "Service not found.");

            return _mapper.Map<ServiceResponseDTO>(service);
        }

        public async Task<ServiceResponseDTO> CreateAsync(CreateServiceDTO dto)
        {
            var entity = _mapper.Map<Service>(dto);

            await _serviceRepository.AddAsync(entity);
            await _serviceRepository.SaveChangesAsync();

            return _mapper.Map<ServiceResponseDTO>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _serviceRepository.GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                throw new NotFoundException("SERVICE", "Service not found.");

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            _serviceRepository.Update(entity);
            await _serviceRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateServiceDTO dto)
        {
            var entity = await _serviceRepository.GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                throw new NotFoundException("SERVICE", "Service not found.");

            _mapper.Map(dto, entity); // <-- ενημερώνει fields
            entity.UpdatedAt = DateTime.UtcNow;

            _serviceRepository.Update(entity);
            await _serviceRepository.SaveChangesAsync();
        }
    }
}
