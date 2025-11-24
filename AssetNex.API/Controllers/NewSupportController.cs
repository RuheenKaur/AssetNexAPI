


using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.NewSupport;
using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;



namespace AssetNex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewSupportController : ControllerBase
    {
        private readonly ISupportRepository supportRepository;
        private readonly ApplicationDbContext dbContext;
        public NewSupportController(ISupportRepository supportRepository, ApplicationDbContext dbContext)
        {
            this.supportRepository = supportRepository;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewSupportDto(CreateNewSupportDto dto)
        {
            var support = new NewSupport
            {
                Id = dto.Id,
                UserName = dto.UserName,
                EmployeeId = dto.EmployeeId,
                Email = dto.Email,
                Department = dto.Department,
                RequestType = dto.RequestType,
            };

            dbContext.NewSupports.Add(support);
            dbContext.SaveChanges();
            return Ok(support);
        }

        [HttpGet]
        public async Task<IActionResult> getAllSupport()
        {
            var getsupport = await supportRepository.getAllSupport();
            var response = getsupport.Select(support => new NewSupport
            {
                Id = support.Id,
                UserName = support.UserName,
                EmployeeId = support.EmployeeId,
                Email = support.Email,
                Department = support.Department,
                RequestType = support.RequestType,
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupportTypeByIdAsync(Guid id)
        {
            var support = await supportRepository.GetSupportByIdAsync(id);
            if (support == null)
                return NotFound();

            var response = new CreateNewSupportDto
            {
                Id = support.Id,
                UserName = support.UserName,
                EmployeeId = support.EmployeeId,
                Email = support.Email,
                Department = support.Department,
                RequestType = support.RequestType,
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateSupport([FromRoute] Guid id, CreateNewSupportDto request)
        {
            var support = new NewSupport
            {
                Id = request.Id,
                UserName = request.UserName,
                EmployeeId = request.EmployeeId,
                Email = request.Email,
                Department = request.Department,
                RequestType = request.RequestType,
            };

            support = await supportRepository.UpdateAsync(support);
            {
                if (support == null)
                {
                    return NotFound();
                }

                var response = new CreateNewSupportDto
                {
                    Id = support.Id,
                    UserName = support.UserName,
                    EmployeeId = support.EmployeeId,
                    Email = support.Email,
                    Department = support.Department,
                    RequestType = support.RequestType,
                };
                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteSupport([FromRoute] Guid id)
        {
            var currentSupport = await supportRepository.GetById(id);

            if (currentSupport == null)
            {
                return NotFound();
            }

            await supportRepository.DeleteAsync(id);
            return NoContent();


        }


    }

}













//cover jwt today as well













