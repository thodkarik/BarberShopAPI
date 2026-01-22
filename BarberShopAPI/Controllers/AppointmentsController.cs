using BarberShopAPI.DTO;
using BarberShopAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDTO request)
        {
            var created = await _appointmentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability([FromQuery] int barberId, [FromQuery] DateTime date)
        {
            var booked = await _appointmentService.GetBookedSlotsAsync(barberId, date);
            return Ok(booked);
        }

    }
}

