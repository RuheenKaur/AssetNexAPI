using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using Microsoft.AspNetCore.Mvc;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetAssignmentsController : ControllerBase
    {
        private readonly IAssetsAssignmentRep _repo;
        private readonly AppDbContext _dbContext;
            

        public AssetAssignmentsController(IAssetsAssignmentRep repo,AppDbContext dbContext)
        {
            _repo = repo;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAll();
            return Ok(result ?? new List<AssetAssignments>());
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAssignedAssetsByUserId(int userId)
        {
            var result = await _repo.GetAssignedAssetsByUserId(userId);
            return Ok(result ?? new List<AssignedAssetDto>());
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignAsset(
        int assetId,
        int assignedToUserId
    )
        {
            var assignment = new AssetAssignments
            {
                AssetId = assetId,
                AssignedOn = DateTime.UtcNow,
                ReturnedOn = null
            };

            await _dbContext.AssetAssignments.AddAsync(assignment);
            await _dbContext.SaveChangesAsync();
            return Ok(assignment);
        }


        [HttpPut("return")]
        public async Task<IActionResult> ReturnAsset(
            [FromQuery] int assetId,
            [FromQuery] int returnedByUserId,
            [FromQuery] string remarks)
        {
            await _repo.ReturnAsync(assetId, returnedByUserId, remarks);
            return Ok(new { message = "Asset returned successfully" });
        }

        [HttpGet("history/{assetId}")]
        public async Task<IActionResult> GetHistory(int assetId)
        {
            var result = await _repo.GetHistory(assetId);
            return Ok(result ?? new List<AssetsHistory>());

        }

    }

}


