using BarberShopAPI.DTO;
using BarberShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDTO request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _appointmentService.CreateAsync(userId, request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Receptionist,Barber,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailsDTO>> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var dto = await _appointmentService.GetByIdAsync(id, userId);
            return Ok(dto);
        }

        [HttpGet("availability")]
        public async Task<ActionResult<List<AvailabilitySlotDTO>>> GetAvailability(
            [FromQuery] DateOnly date,
            [FromQuery] int barberId,
            [FromQuery] int serviceId)
        {
            var slots = await _appointmentService.GetAvailabilityAsync(date, barberId, serviceId);
            return Ok(slots);
        }

        [Authorize(Roles = "Receptionist,Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentStatusDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _appointmentService.UpdateStatusAsync(id, userId, dto.Status);
            return NoContent();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _appointmentService.GetMyAppointmentsAsync(userId);

            return Ok(result);
        }



    }
}

