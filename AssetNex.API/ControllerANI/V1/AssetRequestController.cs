using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.Asset;
using AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.AssetRequests;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using Serilog;
using System;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static Dropbox.Api.Files.SearchMatchType;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetRequestsController : ControllerBase
    {
        private readonly IAssetsRequestsRep _repo;
        private readonly AppDbContext _appDbContext;

        public AssetRequestsController(IAssetsRequestsRep repo, AppDbContext appDbContext)
        {
            _repo = repo;
            _appDbContext = appDbContext;
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses([FromQuery] string category)
        {
            var statuses = await _appDbContext.StatusMaster
                .Where(s => s.StatusCategory == category)
                .Select(s => new {
                    id = s.Id,
                    name = s.StatusName
                })
                .ToListAsync();

            return Ok(statuses);
        }

        [HttpPatch("{id}/status")]
       
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusDto dto)
        {
            var request = await _appDbContext.AssetRequests.FindAsync(id);
            if (request == null)
                return NotFound();

            request.StatusId = dto.StatusId;
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("create")]
           
            public async Task<IActionResult> Create([FromBody] CreateAssetRequestDto dto)
            {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim missing");
            }

            var userId = int.Parse(userIdClaim.Value);

            var user = await _appDbContext.Users.FindAsync(userId);
                if (user == null)
                    return Unauthorized();

                var request = new AssetRequests
                {
                    
                    AssetId = dto.AssetId,
                    RequestedAssetType = dto.RequestedAssetType,
                    Reason = dto.Reason,
                    UserId = userId,
                    StatusId = 11,  // Pending
                    RequestedOn = DateTime.UtcNow
                };

                await _repo.CreateAsync(request);
                return Ok(new { message = "Asset request submitted successfully" });
            }

          


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();  // ← this one
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _repo.Get(id);
            if (result == null)
                return NotFound($"Asset Request ID {id} not found");
            return Ok(result);
        }
        [HttpGet("user/{userId}")]
       
        public async Task<IActionResult> GetByUser(int userId)
        {
            var requests = await _appDbContext.AssetRequests
                .Include(r => r.Status)
                .Include(r => r.Asset)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestedOn)
                .Select(r => new
                {
                    r.Id,
                    r.RequestedAssetType,
                    r.Reason,
                    Status = r.Status.StatusName,
                    r.StatusId,
                    Asset = r.Asset != null ? r.Asset.AssetTag : "—",
                    r.RequestedOn
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] AssetRequests model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != model.Id)
                return BadRequest("ID mismatch between URL and body.");
            var updated = await _repo.Update(model);
            return Ok(updated);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.Delete(id);
            if (!deleted)
                return NotFound($"Asset Request ID {id} not found");
            return Ok($"Request ID {id} removed successfully.");
        }

    }
     
}


