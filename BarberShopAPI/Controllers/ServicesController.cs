using BarberShopAPI.Data;
using BarberShopAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            var created = await _serviceService.CreateAsync(service);
            return CreatedAtAction(nameof(GetServiceById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, Service service)
        {
            await _serviceService.UpdateAsync(id, service);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _serviceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
