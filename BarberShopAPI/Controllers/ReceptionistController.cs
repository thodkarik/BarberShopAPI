using BarberShopAPI.DTO;
using BarberShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/receptionist")]
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistService _service;

        public ReceptionistController(IReceptionistService service)
        {
            _service = service;
        }

        [HttpGet("appointments")]
        public async Task<ActionResult<List<ReceptionistAppointmentDTO>>> GetAppointments([FromQuery] DateTime? date = null)
        {
            var list = await _service.GetAppointmentsAsync(date);
            return Ok(list);
        }

        [HttpPut("appointments/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReceptionistAppointmentStatusDTO dto)
        {
            await _service.UpdateAppointmentStatusAsync(id, dto.Status);
            return NoContent();
        }
    }
}
