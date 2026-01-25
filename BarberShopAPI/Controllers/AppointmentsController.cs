using BarberShopAPI.DTO;
using BarberShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "Customer,Receptionist,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDTO request)
        {
            var created = await _appointmentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "Receptionist,Barber,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _appointmentService.GetByIdAsync(id);
            return Ok(dto);
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability([FromQuery] int barberId, [FromQuery] DateTime date)
        {
            var booked = await _appointmentService.GetBookedSlotsAsync(barberId, date);
            return Ok(booked);
        }

        [Authorize(Roles = "Receptionist,Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentStatusDTO request)
        {
            await _appointmentService.UpdateStatusAsync(id, request.Status);
            return NoContent();
        }



    }
}

