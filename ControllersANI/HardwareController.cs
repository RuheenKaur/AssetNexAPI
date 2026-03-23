
using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.Hardware;
using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNex.API.Controllers

{
    [ApiController]
    [Route("api/[controller]")]


    public class HardwareController : ControllerBase
    {
        private readonly IHardwareRepository hardwareRepository;
        private readonly ApplicationDbContext dbContext;

        public HardwareController(IHardwareRepository hardwareRepository, ApplicationDbContext dbContext)
        {
            this.hardwareRepository = hardwareRepository;
            this.dbContext = dbContext;

        }

        [HttpGet]
        public async Task<IActionResult> getAllHardware()
        {
            var assets = await hardwareRepository.getAllHardware();
            var response = assets.Select(asset => new Hardware
            {
                Id = asset.Id,
                ProblemDescription = asset.ProblemDescription,
                AssetTypeId = asset.AssetTypeId,
                SerialNumber = asset.SerialNumber,
                AssetName = asset.AssetName,
                AssetType = asset.AssetType,
                DateSubmitted = asset.DateSubmitted,
                DateOfIssue = asset.DateOfIssue,
                WarrantyDate = asset.DateSubmitted
            }).ToList();

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateHardware(CreateHardwareDto dto)
        {
            var asset = new Hardware
            {
                Id = Guid.NewGuid(),
                ProblemDescription = dto.ProblemDescription,
                AssetTypeId = dto.AssetTypeId,
                SerialNumber = dto.SerialNumber,
                AssetName = dto.AssetName,
                AssetType = dto.AssetType,
                DateSubmitted = dto.DateSubmitted,
                DateOfIssue = dto.DateOfIssue,
                WarrantyDate = dto.WarrantyDate,
            };

            dbContext.Hardware.Add(asset);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHardwareById(Guid id)
        {
            var asset = await hardwareRepository.GetHardwareByIdAsync(id);

            if (asset == null)
                return NotFound();

            var response = new CreateHardwareDto
            {
                Id = asset.Id,
                ProblemDescription = asset.ProblemDescription,
                AssetTypeId = asset.AssetTypeId,
                SerialNumber = asset.SerialNumber,
                AssetName = asset.AssetName,
                AssetType = asset.AssetType,
                DateSubmitted = asset.DateSubmitted,
                DateOfIssue = asset.DateOfIssue,
                WarrantyDate = asset.WarrantyDate,
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> EditHardware([FromRoute] Guid id, CreateHardwareDto request)
        {
            var asset = new Hardware
            {
                Id = request.Id,
                ProblemDescription = request.ProblemDescription,
                AssetTypeId = request.AssetTypeId,
                SerialNumber = request.SerialNumber,
                AssetName = request.AssetName,
                AssetType = request.AssetType,
                DateSubmitted = request.DateSubmitted,
                DateOfIssue = request.DateOfIssue,
                WarrantyDate = request.WarrantyDate,
            };

            asset = await hardwareRepository.UpdateAsync(asset);

            {
                if (asset == null)
                {
                    return NotFound();
                }
                var response = new CreateHardwareDto
                {
                    Id = asset.Id,
                    ProblemDescription = asset.ProblemDescription,
                    AssetTypeId = asset.AssetTypeId,
                    SerialNumber = asset.SerialNumber,
                    AssetName = asset.AssetName,
                    AssetType = asset.AssetType,
                    DateSubmitted = asset.DateSubmitted,
                    DateOfIssue = asset.DateOfIssue,
                    WarrantyDate = asset.WarrantyDate,
                };
                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsset([FromRoute] Guid id)
        {
            {
                var currentAsset = await hardwareRepository.GetById(id);
                if (currentAsset == null)
                {
                    return NotFound();
                }
                await hardwareRepository.DeleteAsync(id);
                return NoContent();
            }
        }
    }
}

