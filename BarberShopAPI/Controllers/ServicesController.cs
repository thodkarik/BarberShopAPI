using BarberShopAPI.Data;
using BarberShopAPI.DTO;
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
        public async Task<ActionResult<List<ServiceResponseDTO>>> GetAll()
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponseDTO>> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            return Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponseDTO>> Create(CreateServiceDTO dto)
        {
            var created = await _serviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateServiceDTO dto)
        {
            await _serviceService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceService.DeleteAsync(id);
            return NoContent();
        }
    }
}
