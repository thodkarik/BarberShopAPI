using BarberShopAPI.Data;
using BarberShopAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _context.Services
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (service == null)
            {
                throw new NotFoundException("SERVICE_NOT_FOUND", $"Service with ID {id} not found.");
            }
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            if (string.IsNullOrWhiteSpace(service.Name))
            {
                throw new BadRequestException("INVALID_SERVICE_NAME", "Service name cannot be empty.");
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, Service updatedService)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (id != updatedService.Id)
            {
                throw new BadRequestException("ID_MISMATCH", "Id mismatch");
            }

            if (service == null)
            {
                throw new NotFoundException("SERVICE_NOT_FOUND", $"Service with ID {id} not found.");
            }

            service.Name = updatedService.Name;
            service.DurationMinutes = updatedService.DurationMinutes;
            service.Price = updatedService.Price;
            service.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (service == null)
            {
                throw new NotFoundException("SERVICE_NOT_FOUND", $"Service with ID {id} not found.");
            }

            service.IsDeleted = true;
            service.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
