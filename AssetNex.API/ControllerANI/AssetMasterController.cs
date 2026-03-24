using AssetNex.API.Models.DomainModel;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [ApiController]

    [Route("api/[controller]")]
    public class AssetsMasterController : ControllerBase
    {
        private readonly IAssetsMasterRep _repo;
        private readonly AppDbContext _context;

        public AssetsMasterController(IAssetsMasterRep repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
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
        public async Task<IActionResult> Create([FromBody] AssetsMaster asset)
        {
            if (asset == null) return BadRequest("Asset is null.");

            try
            {
                var created = await _repo.AddAsync(asset);
                return CreatedAtRoute("GetAssetById", new { id = created.Id }, created);
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

        [HttpPut("{id:int}")]

        public async Task<IActionResult> Update(int id, [FromBody] AssetsMaster asset)

        {
            if (asset == null)
                return BadRequest("Asset is null");
            if (id != asset.Id)
            {
                asset.Id = id;
            }

            try
            {
                var updated = await _repo.UpdateAsync(asset);
                return Ok(updated);
            }

            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating asset: {ex.Message}");
            }
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
    }

}

        



