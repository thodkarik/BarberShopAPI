using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReceptionistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReceptionistsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReceptionistListItemDTO>>> GetAll()
        {
            var list = await _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && u.UserRole == UserRole.Receptionist)
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .Select(u => new ReceptionistListItemDTO
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName
                })
                .ToListAsync();

            return Ok(list);
        }
    }
}
