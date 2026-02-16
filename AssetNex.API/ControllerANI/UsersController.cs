using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.User;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRep _repo;
        private readonly AppDbContext _context;
   
        public UsersController(IUsersRep repo, AppDbContext context)
        {
            _repo = repo;

            _context = context;
            
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());


        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_repo.GetById(id));
    
        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUserByEmailAsync(string email)
        {
            var users = await _repo.GetByEmailAsync(email);

            if (users == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(users);
        }

        [HttpPost]
        public IActionResult Create(Users user)
        {
            _repo.Create(user);
            return Ok("User Created");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
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
            var user = await _repo.GetUserProfileAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

     


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Users user)
        {
            if (user == null) return BadRequest();

            if (id != user.Id)
                user.Id = id;

            var updated = await _repo.UpdateAsync(user);
            return Ok(updated);
        }

        [HttpPut("deactivate/{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _repo.DeactivateUserAsync(id);
            if (!success) return NotFound();

            return Ok();
        }
    }
}



 
    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //{
    //    var users = await _repo.GetAllAsync();
    //    return Ok(users);
    //}

  

