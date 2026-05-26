using AssetNex.API.Models.DTOANI.Assets;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsMasterController : ControllerBase
    {
        private readonly IAssetsMasterRep _repo;
        private readonly AppDbContext _context;
        private readonly IAssetsHistoryRep _historyRepo;

        public AssetsMasterController(IAssetsMasterRep repo, AppDbContext context, IAssetsHistoryRep historyRepo)
        {
            _repo = repo;
            _context = context;
            _historyRepo = historyRepo;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _repo.GetAllAsync();
            return Ok(assets);
        }


        [HttpGet("{userId:int}", Name = "GetAssetById")]
        public async Task<IActionResult> Get(int userId)
        {
            var asset = await _repo.GetAsync(userId);
            if (asset == null) return NotFound();
            return Ok(asset);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssetDto dto)
        {
            if (dto == null) return BadRequest("Asset data is null.");

            var asset = new AssetsMaster
            {
                AssetTag = dto.AssetTag,
                AssetType = dto.AssetType,
                Brand = dto.Brand,
                Model = dto.Model ?? string.Empty,
                SerialNumber = dto.SerialNumber ?? string.Empty,
                RAM_GB = dto.RAM_GB ?? string.Empty,
                Storage_GB = dto.Storage_GB ?? string.Empty,
                StatusId = dto.StatusId > 0 ? dto.StatusId : 1,
                PurchaseCost = dto.PurchaseCost,
                WarrantyDate = dto.WarrantyDate,
                PurchaseDate = dto.PurchaseDate ?? DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                DepartmentId = dto.DepartmentId > 0 ? dto.DepartmentId : 1
            };

            try
            {
                var created = await _repo.AddAsync(asset);
                return Ok(new { message = "Asset created successfully", id = created.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving asset: {ex.Message}");
            }
        }

        [HttpGet("status/{statusId:int}", Name = "GetAssetsByStatusId")]
        public async Task<IActionResult> GetAsyncStatus(int statusId)
        {
            var asset = await _repo.GetAsync(statusId);
            if (asset == null) return NotFound();
            return Ok(asset);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAssetDto dto)
        {

            var asset = await _context.AssetMaster.FindAsync(id);
            if (asset == null) return NotFound();

            asset.AssetTag = dto.AssetTag;
            asset.AssetType = dto.AssetType;
            asset.Brand = dto.Brand;
            asset.Model = dto.Model;
            asset.SerialNumber = dto.SerialNumber;
            asset.StatusId = dto.StatusId;

            await _context.SaveChangesAsync();


            try
            {
                var adminName = User?.FindFirst(System.Security.Claims.ClaimTypes.Email)
                    ?.Value ?? "Admin";

                await _historyRepo.CreateAsync(new AssetsHistory
                {
                    AssetId = id,
                    UserId = 0,
                    EventType = "Updated",
                    Remarks = $"Asset details updated. Status set to {dto.StatusId}",
                    PerformedAt = DateTime.UtcNow,
                    StatusId = dto.StatusId,
                    ModifiedBy = adminName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"History log failed (update): {ex.Message}");
            }

            return Ok(new { message = "Asset updated" });
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetAssetsPaged(
        int page = 1,
        int pageSize = 10,
        string? search = "")
        {

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _repo.GetAssetsPagedAsync(page, pageSize, search ?? "");

            return Ok(new
            {
                data = result.Data,
                pagination = new
                {
                    currentPage = result.Page,
                    pageSize = result.PageSize,
                    totalCount = result.TotalCount,
                    totalPages = result.TotalPages
                }
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("debug-data")]
        public async Task<IActionResult> DebugData()
        {
            var total = await _context.AssetMaster.CountAsync();
            var sample = await _context.AssetMaster.Take(5).ToListAsync();

            return Ok(new
            {
                totalAssets = total,
                sampleData = sample.Select(x => new
                {
                    x.Id,
                    x.AssetTag,
                    x.Brand,
                    x.AssetType
                })
            });

        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAssetStatusDto dto)
        {
            var asset = await _context.AssetMaster.FindAsync(id);
            if (asset == null) return NotFound();
            asset.StatusId = dto.StatusId;
            await _context.SaveChangesAsync();
            return Ok();
        }



    }

}





