using BarberShopAPI.Core.Enums;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;
using BarberShopAPI.Exceptions;
using BarberShopAPI.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == request.Username && !u.IsDeleted);

            if (user == null)
                return Unauthorized();

            if (!PasswordHashUtil.Verify(request.Password, user.Password))
                return Unauthorized();

            var expiresInMinutes = request.KeepLoggedIn ? 1440 : 60;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] CustomerSignupDTO request)
        {
            var usernameExists = await _context.Users.AnyAsync(u => u.Username == request.Username && !u.IsDeleted);
            if (usernameExists)
                throw new ConflictException("USER", "Username already exists");

            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email && !u.IsDeleted);
            if (emailExists)
                throw new ConflictException("USER", "Email already exists");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = PasswordHashUtil.Hash(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserRole = UserRole.Customer
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserId = user.Id
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new
            {
                user.Id,
                user.Username,
                user.Email,
                Role = user.UserRole.ToString()
            });
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
                throw new NotFoundException("USER", $"User with id {id} not found");

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                Role = user.UserRole.ToString()
            });
        }
    }
}
