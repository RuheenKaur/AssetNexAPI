using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.User;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRep _repo;

        public UsersController(IUsersRep repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repo.GetAllAsync();
            var result = users.Select(u => new
            {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                contact = u.Contact,
                role = u.Role,
                createdOn = u.createdOn,
                isActive = u.IsActive
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null) return NotFound();

            var dto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Contact = user.Contact
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
    [FromBody] UserDto dto,
    [FromServices] UserManager<ApplicationUser> userManager)
        {

            var user = new Users
            {
                Name = dto.Name,
                Email = dto.Email,
                Contact = dto.Contact ?? string.Empty,
                Role = dto.Role ?? "User",
                PasswordHash = "managed-by-identity",
                createdOn = DateTime.UtcNow,
                IsActive = true
            };
            await _repo.CreateAsync(user);


            var defaultPassword = "Welcome@123";
            var identityUser = new ApplicationUser
            {
                UserName = dto.Email.Trim(),
                Email = dto.Email.Trim(),
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, defaultPassword);
            if (!result.Succeeded)
            {

                Console.WriteLine($"Identity creation failed for {dto.Email}: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else
            {
                await userManager.AddToRoleAsync(identityUser, dto.Role ?? "User");
            }

            return Ok(new { message = "User created", id = user.Id, defaultPassword });
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return Ok(new { message = "User deleted" }); 
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _repo.DeactivateUserAsync(id);
            if (!success) return NotFound(new { message = "User not found" });
            return Ok(new { message = "User deactivated" });
        }

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> Profile(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Contact = dto.Contact;

            await _repo.UpdateAsync(existing);

            return Ok("Updated successfully");
        }
    }
}