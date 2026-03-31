﻿using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AssetNex.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IOptions<JwtSettings> jwtOptions,

            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = jwtOptions.Value;
            _logger = logger;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Invalid email or password");


            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                };


            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)

            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddHours(_jwtSettings.ExpiryHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var refreshToken = new RefreshTokenModel
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                Expiry = DateTime.UtcNow.AddDays(7)
            };


            var db = HttpContext.RequestServices.GetRequiredService<AuthDbContext>();
            db.RefreshTokenModel.Add(refreshToken);
            await db.SaveChangesAsync();

            var userRoless = await _userManager.GetRolesAsync(user);
            var primaryRole = userRoless.FirstOrDefault();



            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshToken.Token,
                expiration = token.ValidTo,
                email = user.Email,
                role = primaryRole,
                id = user.Id,
                name = user.UserName
            });



        }

        //[HttpPost("assign-role")]
        //public async Task<IActionResult> AssignRole([FromQuery] string email, [FromQuery] string role)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null) return NotFound("User not found");

        //    var result = await _userManager.AddToRoleAsync(user, role);
        //    if (result.Succeeded) return Ok("Role assigned");

        //    return BadRequest(result.Errors);
        //}



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

            var user = new IdentityUser
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


            var claims = new List<Claim>
        {
        new Claim(ClaimTypes.Email, user.Email!)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
            claims: claims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );


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
            public int ExpiryHours { get; set; }
        }



    }

}