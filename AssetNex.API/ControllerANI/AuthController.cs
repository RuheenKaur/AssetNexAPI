using AssetNex.API.Data;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
﻿using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using AssetNex.API.Models.DTOANI.Register;
using AssetNex.API.Models.DTO.Register;


namespace AssetNex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;
        private readonly AppDbContext _appDb;
     public AuthController(
     UserManager<ApplicationUser> userManager,
     IConfiguration configuration,
     IOptions<JwtSettings> jwtOptions,
     ILogger<AuthController> logger,
     AppDbContext appDb)
  
         {
     _userManager = userManager;
     _configuration = configuration;
     _jwtSettings = jwtOptions.Value;
     _logger = logger;
     _appDb = appDb; 
 }

      

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid email or password");

            var customUser = await _appDb.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (customUser == null)
                return Unauthorized("User profile not found");

            var authClaims = new List<Claim>
    {
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, customUser.Id.ToString()),
    };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
                authClaims.Add(new Claim("role", role));
            }

            var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddHours(_jwtSettings.ExpiryHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                authSigningKey, SecurityAlgorithms.HmacSha256)
            );


            //var refreshToken = new RefreshTokenModel
            //{
            //    Token = GenerateRefreshToken(),
            //    UserId = user.Id,
            //    Expiry = DateTime.UtcNow.AddDays(7)
            //};

            //var db = HttpContext.RequestServices.GetRequiredService<AuthDbContext>();
            //db.RefreshTokenModel.Add(refreshToken);
            //await db.SaveChangesAsync();

            var refreshToken = GenerateRefreshToken();

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            var primaryRole = userRoles.FirstOrDefault();

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
               
                expiration = token.ValidTo,
                email = user.Email,
                role = primaryRole,
                id = user.Id,
                numericId = customUser != null ? customUser.Id : 0,
                name = customUser != null ? customUser.Name : user.Email,
                contact = customUser != null ? customUser.Contact : user.Contact
            });
        }


        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromQuery] string roleName,
                                            [FromServices] RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                return Ok($"Role '{roleName}' already exists.");

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
                return Ok($"Role '{roleName}' created successfully.");

            return BadRequest(result.Errors);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var user = new ApplicationUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Errors = identityResult.Errors.Select(e => e.Description)
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");

            // NEW — create matching row in custom Users table 
            var customUser = new Users
            {
                Name = request.Name ?? request.Email,
                Email = request.Email,
                Contact = request.Contact ?? ""
            };
            _appDb.Users.Add(customUser);
            await _appDb.SaveChangesAsync();

            if (!roleResult.Succeeded)
            {
                return Ok(new RegisterResponseDto
                {
                    Success = true,
                    Message = "User registered successfully, but role assignment failed. Contact admin."
                });
            }

            return Ok(new RegisterResponseDto
            {
                Success = true,
                Message = "User registered successfully"
            });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string newPassword)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound("User Not Found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
                return Ok(new { message = $"Password reset successful for {email}" });

            else
                return BadRequest(result.Errors);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var db = HttpContext.RequestServices.GetRequiredService<AuthDbContext>();
            var storedToken = db.RefreshTokenModel.FirstOrDefault(rt => rt.Token == refreshToken);

            if (storedToken == null || !storedToken.IsActive)
                return Unauthorized("Invalid refresh token");

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (user == null)
                return Unauthorized("Invalid user");


            // Build JWT without relying on Claim type to avoid type ambiguity
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: null,
                notBefore: null,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours)
            );

            payload[ClaimTypes.Email] = user.Email ?? string.Empty;

            var token = new JwtSecurityToken(header, payload);


            if (user == null)
                return Accepted("The user needs to be validated");


            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        public class JwtSettings

        {
            public string Key { get; set; } = string.Empty;
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public int ExpiryHours { get; set; } = 8;
        }



    }

}