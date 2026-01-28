using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}

