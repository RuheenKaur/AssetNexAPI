
using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.EWaste;
using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;


namespace AssetNex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EWasteController : ControllerBase
    {
        private readonly IEWasteRepository ewasteRepository;
        private readonly ApplicationDbContext dbContext;

        public EWasteController(IEWasteRepository ewasteRepository, ApplicationDbContext dbContext)
        {
            this.ewasteRepository = ewasteRepository;
            this.dbContext = dbContext;
        }

        [HttpGet("disposable-assets")]
        public async Task<IActionResult> GetDisposedAssets()
        {
            var assets = await ewasteRepository.GetDisposedAssets();

            var response = assets.Select(ewaste => new EWaste
            {
                Id = Guid.NewGuid(),
                AssetTypeId = ewaste.AssetTypeId,
                AssetName = ewaste.AssetName,
                Reason = ewaste.Reason,
                DisposedOn = ewaste.DisposedOn,
                DisposedBy = ewaste.DisposedBy,
                Condition = ewaste.Condition,
                DateOfIssue = ewaste.DateOfIssue,
                WarrantyDate = ewaste.WarrantyDate,
                AssetType = ewaste.AssetType,
                Status = ewaste.Status,

            }).ToList();

            return Ok(response);
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetDisposedAssetsById()
        {
            var disposed = await ewasteRepository.GetDisposedAssetsAsync();
            return Ok(disposed);

        }


        [HttpPost("disposable-assets")]
        public IActionResult DisposedAsset(DisposeAssetRequestDto dto)
        {

            var waste = new EWaste
            {
                AssetTypeId = dto.AssetTypeId,
                AssetName = dto.AssetName,
                Reason = dto.Reason,
                DisposedOn = dto.DisposedOn,
                DisposedBy = dto.DisposedBy,
                Condition = dto.Condition,
                DateOfIssue = dto.DateOfIssue,
                WarrantyDate = dto.WarrantyDate,
                AssetType = dto.AssetType,
                Status = dto.Status,
            };

            dbContext.EWastes.Add(waste);
            dbContext.SaveChanges();
            return Ok(waste);
        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateDisposedAssets([FromRoute] Guid id, UpdateEDisposeAssetRequestDto request)
        {
            var ewaste = new EWaste
            {
                Id = Guid.NewGuid(),
                AssetName = request.AssetName,
                AssetType = request.AssetType,
                Reason = request.Reason,
                DisposedOn = request.DisposedOn,
                DisposedBy = request.DisposedBy,
                Condition = request.Condition,
                DateOfIssue = request.DateOfIssue,
                WarrantyDate = request.WarrantyDate,
                Status = request.Status

            };

            ewaste = await ewasteRepository.UpdateDisposedAssetAsync(ewaste);

            {
                if (ewaste == null)
                {
                    return NotFound();

                }

                var response = new UpdateEDisposeAssetRequestDto
                {
                    Id = ewaste.Id,
                    AssetName = ewaste.AssetName,
                    AssetType = ewaste.AssetType,
                    Reason = ewaste.Reason,
                    DisposedOn = ewaste.DisposedOn,
                    DisposedBy = ewaste.DisposedBy,
                    Condition = ewaste.Condition,
                    DateOfIssue = ewaste.DateOfIssue,
                    WarrantyDate = ewaste.WarrantyDate,
                    Status = ewaste.Status


                };


                return Ok(response);

            }

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteDisposableAssetAsync([FromRoute] Guid id)
        {
            {
                var currentAsset = await ewasteRepository.GetEWasteTypeByIdAsync(id);

                if (currentAsset == null)
                {
                    return NotFound();
                }

                await ewasteRepository.DeleteDisposableAssetAsync(id);

                return NoContent();

            }

        }
    }

}


