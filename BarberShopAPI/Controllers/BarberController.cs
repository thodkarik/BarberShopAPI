using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/barber")]
    [Authorize(Roles = "Barber")]
    public class BarberController : ControllerBase
    {
        private readonly IBarberService _barberService;

        public BarberController(IBarberService barberService)
        {
            _barberService = barberService;
        }

        [HttpGet("appointments")]
        public async Task<ActionResult<List<BarberAppointmentDTO>>> GetMyAppointments([FromQuery] DateTime? date = null)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var list = await _barberService.GetMyAppointmentsAsync(userId, date);
            return Ok(list);
        }
    }
}
