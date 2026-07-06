using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetAssignmentsController : ControllerBase
    {
        private readonly IAssetsAssignmentRep _repo;
        private readonly AppDbContext _dbContext;
        private readonly IAssetsHistoryRep _historyRepo;

        public AssetAssignmentsController(
            IAssetsAssignmentRep repo,
            AppDbContext dbContext,
            IAssetsHistoryRep historyRepo)
        {
            _repo = repo;
            _dbContext = dbContext;
            _historyRepo = historyRepo;
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

        [HttpPut("return")]
        public async Task<IActionResult> ReturnAsset(
            [FromQuery] int assetId,
            [FromQuery] int returnedByUserId,   
            [FromQuery] string remarks)
        {
            var adminName = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Admin";

            await _repo.ReturnAsync(assetId, returnedByUserId, remarks);
            return Ok(new { message = "Asset returned successfully" });
        }

        [HttpGet("history/{assetId}")]
        public async Task<IActionResult> GetHistory(int assetId)
        {
            var adminName = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Admin";
            var result = await _repo.GetHistory(assetId);
            return Ok(result ?? new List<AssetsHistory>());
        }



        [HttpPost("assign")]
        public async Task<IActionResult> AssignAsset(
    [FromQuery] int assetId,
    [FromQuery] int assignedToUserId)
        {
            var adminName = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Admin";

            var asset = await _dbContext.AssetMaster.FindAsync(assetId);
            if (asset == null) return NotFound("Asset not found");

            var assignment = new AssetAssignments
            {
                AssetId = assetId,
                UserId = assignedToUserId,
                AssignedOn = DateTime.UtcNow,
                ReturnedOn = null,
                AssetAssigned = "Assigned"
            };

            await _dbContext.AssetAssignments.AddAsync(assignment);
            asset.StatusId = 3; // Assigned
            await _dbContext.SaveChangesAsync();

            await _historyRepo.CreateAsync(new AssetsHistory
            {
                AssetId = assetId,
                UserId = assignedToUserId,
                EventType = "Assigned",
                Remarks = "Asset assigned to user",
                PerformedAt = DateTime.UtcNow,
                StatusId = 3,
                ModifiedBy = adminName
            });

            return Ok(new { message = "Asset assigned successfully" });
        }

        [HttpPut("unassign")]
        public async Task<IActionResult> UnassignAsset([FromQuery] int assetId)
        {
            var adminName = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Admin";

            var assignment = await _dbContext.AssetAssignments
                .Where(a => a.AssetId == assetId && a.ReturnedOn == null)
                .OrderByDescending(a => a.AssignedOn)
                .FirstOrDefaultAsync();

            if (assignment == null)
                return NotFound("No active assignment found for this asset");

            var userId = assignment.UserId;
            assignment.ReturnedOn = DateTime.UtcNow;

            var asset = await _dbContext.AssetMaster.FindAsync(assetId);
            if (asset != null) asset.StatusId = 2; // Available

            await _dbContext.SaveChangesAsync();

            await _historyRepo.CreateAsync(new AssetsHistory
            {
                AssetId = assetId,
                UserId = userId,
                EventType = "Returned",
                Remarks = "Asset unassigned",
                PerformedAt = DateTime.UtcNow,
                StatusId = 2, // Available
                ModifiedBy = adminName
            });

            return Ok(new { message = "Asset unassigned successfully" });
        }

        [HttpPost("reassign")]
        public async Task<IActionResult> ReassignAsset(
            [FromQuery] int assetId,
            [FromQuery] int newUserId)
        {
            var adminName = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "Admin";

            var current = await _dbContext.AssetAssignments
                .Where(a => a.AssetId == assetId && a.ReturnedOn == null)
                .OrderByDescending(a => a.AssignedOn)
                .FirstOrDefaultAsync();

            if (current != null)
                current.ReturnedOn = DateTime.UtcNow;

            var newAssignment = new AssetAssignments
            {
                AssetId = assetId,
                UserId = newUserId,
                AssignedOn = DateTime.UtcNow,
                ReturnedOn = null,
                AssetAssigned = "Assigned"
            };

            await _dbContext.AssetAssignments.AddAsync(newAssignment);

            var asset = await _dbContext.AssetMaster.FindAsync(assetId);
            if (asset != null) asset.StatusId = 3; // Assigned

            await _dbContext.SaveChangesAsync();

            await _historyRepo.CreateAsync(new AssetsHistory
            {
                AssetId = assetId,
                UserId = newUserId,
                EventType = "ReAssigned",
                Remarks = "Asset reassigned to new user",
                PerformedAt = DateTime.UtcNow,
                StatusId = 3, // Assigned
                ModifiedBy = adminName
            });

            return Ok(new { message = "Asset reassigned successfully" });
        }
    }
}
