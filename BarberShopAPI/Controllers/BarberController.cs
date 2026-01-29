using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BarbersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BarbersController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<BarberListDTO>>> GetAll()
        {
            var list = await _context.Barbers
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.LastName).ThenBy(b => b.FirstName)
                .Select(b => new BarberListDTO
                {
                    Id = b.Id,
                    FullName = b.FirstName + " " + b.LastName
                })
                .ToListAsync();

            return Ok(list);
        }

        [Authorize(Roles = "Barber")]
        [HttpGet("appointments")]
        public async Task<ActionResult<List<BarberAppointmentItemDTO>>> GetMyAppointments()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var barber = await _context.Barbers
                .AsNoTracking()
                .FirstOrDefaultAsync(b => !b.IsDeleted && b.UserId == userId);

            if (barber == null)
                return NotFound(new { message = "Barber profile not found for this user." });

            var items = await _context.Appointments
                .AsNoTracking()
                .Where(a => !a.IsDeleted && a.BarberId == barber.Id)
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .OrderByDescending(a => a.AppointmentDateTime)
                .Select(a => new BarberAppointmentItemDTO
                {
                    Id = a.Id,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status,
                    ServiceName = a.Service.Name,
                    CustomerName = a.Customer.FirstName + " " + a.Customer.LastName
                })
                .ToListAsync();

            return Ok(items);
        }
    }
}