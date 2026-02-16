using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.Asset;
using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNex.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase

    {
        private readonly IAssetsRepository assetsRepository;

        private readonly ApplicationDbContext dbContext;

        public AssetsController(IAssetsRepository assetsRepository, ApplicationDbContext dbContext)
        {

            this.assetsRepository = assetsRepository;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> getAllAssets()
        {
            var assets = await assetsRepository.getAllAssets();
            var response = assets.Select(asset => new AssetInfo
            {
                Id = asset.Id,
                Name = asset.Name,
                SerialNumber = asset.SerialNumber,
                Department = asset.Department,
                DateOfIssue = asset.DateOfIssue,
                WarrantyDate = asset.WarrantyDate,
                User = asset.User,
                UserId = asset.UserId,
                Status = asset.Status,

                AssetTypeId = asset.AssetTypeId,
            })
            .ToList();

            return Ok(response);
        }



        [HttpPost]
        public IActionResult CreateAsset(CreateInventoryDto dto)
        {
            var asset = new AssetInfo
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Department = dto.Department,
                AssetType = dto.AssetType,
                SerialNumber = dto.SerialNumber,
                DateOfIssue = dto.DateOfIssue,
                WarrantyDate = dto.WarrantyDate,
                AssetTypeId = dto.AssetTypeId,
                UserId = dto.UserId,
                User = dto.User,
                Status = dto.Status
            };

            dbContext.AssetInfo.Add(asset);
            dbContext.SaveChanges();
            return Ok(asset);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(Guid id)
        {
            var asset = await assetsRepository.GetAssetByIdAsync(id);

            if (asset == null)
                return NotFound();

            var response = new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                SerialNumber = asset.SerialNumber,
                Department = asset.Department,
                DateOfIssue = asset.DateOfIssue,
                WarrantyDate = asset.WarrantyDate,
                Status = asset.Status,
                UserId = asset.UserId,
                AssetTypeName = asset.AssetType?.Name,

            };

            return Ok(response);
        }


        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> EditAsset([FromRoute] Guid id, UpdateAssetRequestDto request)
        {
            var asset = new AssetInfo
            {

                Name = request.Name,
                SerialNumber = request.SerialNumber,
                Department = request.Department,
                DateOfIssue = request.DateOfIssue,
                WarrantyDate = request.WarrantyDate,
                AssetTypeId = request.AssetTypeId,
                Status = request.Status,
                UserId = request.UserId,


            };

            asset = await assetsRepository.UpdateAsync(asset);

            {
                if (asset == null)
                {
                    return NotFound();

                }
                var response = new AssetDto
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    SerialNumber = asset.SerialNumber,
                    Department = asset.Department,
                    DateOfIssue = asset.DateOfIssue,
                    WarrantyDate = asset.WarrantyDate,
                    Status = asset.Status,
                    UserId = asset.UserId,

                };

                return Ok(response);

            }

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteAsset([FromRoute] Guid id)
        {
            {
                var currentAsset = await assetsRepository.GetById(id);

                if (currentAsset == null)
                {
                    return NotFound();
                }

                await assetsRepository.DeleteAsync(id);

                return NoContent();

            }
        }
    }

}






