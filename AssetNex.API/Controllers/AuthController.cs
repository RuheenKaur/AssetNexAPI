
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AssetNex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwt;
        private readonly AppDbContext _context;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IConfiguration config, 
            AppDbContext context)
        {
            _userManager = userManager;
            _jwt = config.GetSection("JwtSettings").Get<JwtSettings>()!;
            _context = context;
        }

     

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password required");
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");
            var appUser = await _context.Users
           .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (appUser == null)
                return Unauthorized("User profile not found");
            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPassword)
                return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            var claims = new List<Claim>
{
    new Claim(JwtRegisteredClaimNames.Sub, user.Id),                 
    new Claim("UserId", appUser.Id.ToString()),                  
    new Claim(ClaimTypes.Name, appUser.Name),                   
    new Claim(JwtRegisteredClaimNames.Email, user.Email),
    new Claim(ClaimTypes.Role, role)
};

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwt.Key));

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwt.ExpiryHours),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
 );


            return Ok(new
            {
                id = appUser.Id,               
                identityId = user.Id,              
                name = appUser.Name,         
                email = user.Email,
                role = role,
                accessToken = new JwtSecurityTokenHandler().WriteToken(token)
            });

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password reset successful");
        }



        public class JwtSettings
        {
            public string Key { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public int ExpiryHours { get; set; }
        }
    }

}
