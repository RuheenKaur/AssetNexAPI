using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.SoftwareLicense;
using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SoftwareLicenseController : ControllerBase
    {

        private readonly ISoftwareLicenseRepository licenseRepository;
        private readonly ApplicationDbContext dbContext;
        public SoftwareLicenseController(ISoftwareLicenseRepository licenseRepository, ApplicationDbContext dbContext)
        {
            this.licenseRepository = licenseRepository;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSoftwareLicense()
        {
            var licenses = await licenseRepository.GetAllSoftwareLicense();
            var response = licenses.Select(license => new SoftwareLicenseInfo
            {
                Id = license.Id,
                UserName = license.UserName,
                Request = license.Request,
                EmployeeId = license.EmployeeId,
                SoftwareName = license.SoftwareName,
                OtherSoftware = license.OtherSoftware,
                DateApplied = license.DateApplied,

            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var license = await licenseRepository.GetBySoftwareLicenseIdAsync(id);

            if (license == null)
                return NotFound();
            var response = new SoftwareLicenseDto
            {
                Id = license.Id,
                UserName = license.UserName,
                Request = license.Request,
                EmployeeId = license.EmployeeId,
                SoftwareName = license.SoftwareName,
                OtherSoftware = license.OtherSoftware,
                DateApplied = license.DateApplied,
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(CreateSoftwareLicenseDto dto)
        {
            var license = new SoftwareLicenseInfo
            {
                Id = Guid.NewGuid(),
                SoftwareName = dto.SoftwareName,
                OtherSoftware = dto.OtherSoftware,
                DateApplied = dto.DateApplied,
                Request = dto.Request,
                UserName = dto.UserName,
                EmployeeId = dto.EmployeeId
            };

            dbContext.SoftwareLicenseInfo.Add(license);
            dbContext.SaveChanges();
            return Ok(license);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateSoftwareLicense([FromRoute] Guid id, SoftwareLicenseDto request)
        {
            {
                var license = new SoftwareLicenseInfo
                {

                    Id = Guid.NewGuid(),

                    SoftwareName = request.SoftwareName,
                    OtherSoftware = request.OtherSoftware,
                    DateApplied = request.DateApplied,
                    Request = request.Request,
                    UserName = request.UserName,
                    EmployeeId = request.EmployeeId,
                };

                license = await licenseRepository.UpdateSoftwareLicenseAsync(license);
                {
                    if (license == null)
                    {
                        return NotFound();

                    }
                    var response = new SoftwareLicenseDto
                    {
                        Id = license.Id,
                        SoftwareName = license.SoftwareName,
                        OtherSoftware = license.OtherSoftware,
                        DateApplied = license.DateApplied,
                        Request = license.Request,
                        UserName = license.UserName,
                        EmployeeId = license.EmployeeId,

                    };

                    return Ok(response);
                }

            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingCategory = await licenseRepository.GetBySoftwareLicenseIdAsync(id);

            if (existingCategory == null)
                return NotFound();

            await licenseRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}



