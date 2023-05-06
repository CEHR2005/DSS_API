using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DSS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

namespace DSS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await AuthenticateUser(loginModel);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenString = GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
        }

        private async Task<User> AuthenticateUser(LoginModel loginModel)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == loginModel.Username);

            if (user != null && user.Password == loginModel.Password)
            {
                return user;
            }

            return null;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var header = new JwtHeader(creds)
            {
                // Добавьте идентификатор ключа здесь
                {"kid", _configuration["Jwt:KeyIdentifier"]}
            };

            var payload = new JwtPayload(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                null,
                expires
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                return BadRequest("Пароли не совпадают.");
            }

            if (await _context.User.AnyAsync(u => u.Username == registerModel.Username))
            {
                return BadRequest("Пользователь с таким именем уже существует.");
            }

            var newUser = new User { Username = registerModel.Username, Password = registerModel.Password };
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Пользователь успешно зарегистрирован.");
        }
    }
}
