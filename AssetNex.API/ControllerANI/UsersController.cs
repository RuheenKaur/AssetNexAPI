using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.User;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
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
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            var user = new Users
            {
                Name = dto.Name,
                Email = dto.Email,
                Contact = dto.Contact ?? string.Empty,
                Role = dto.Role ?? "User",
                PasswordHash = "demo123",
                createdOn = DateTime.UtcNow,
                IsActive = true
            };
            await _repo.CreateAsync(user);
            return Ok(new { message = "User created", id = user.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return Ok("User Deleted");
        }

      
        [HttpPost("loginuser")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

          
            if (user.PasswordHash != dto.Password)
                return Unauthorized("Invalid credentials");

            return Ok(new
            {
                userId = user.Id,
                role = user.Role
            });
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

     
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _repo.DeactivateUserAsync(id);
            if (!success) return NotFound();

            return Ok("User Deactivated");
        }
    }
}