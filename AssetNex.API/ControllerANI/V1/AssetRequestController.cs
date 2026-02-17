using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.Asset;
using AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.AssetRequests;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
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
            [Authorize]
            public async Task<IActionResult> Create([FromBody] CreateAssetRequestDto dto)
            {
                var userId = int.Parse(User.FindFirst("userId")!.Value);

                var user = await _appDbContext.Users.FindAsync(userId);
                if (user == null)
                    return Unauthorized();

                var request = new AssetRequests
                {
                    AssetId = dto.AssetId,
                    RequestedAssetType = dto.RequestedAssetType,
                    Reason = dto.Reason,
                    UserId = userId,

                
                    StatusId = 8,

                    RequestedOn = DateTime.UtcNow
                };

                await _repo.CreateAsync(request);
                return Ok(new { message = "Asset request submitted successfully" });
            }

          
           
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAll();
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

        [HttpPost]
        public async Task<IActionResult> Create(AssetRequestDto dto)
        {
            var request = new AssetRequests
            {
                UserId = dto.UserId,
                RequestedAssetType = dto.RequestedAssetType,
                Reason = dto.Reason,
                StatusId = 1,
                RequestedOn = DateTime.Now
            };

            await _repo.Add(request);
            return Ok(request);
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


/// api / v1 / asset - requests
/// api / v2 / asset - requests




//using Microsoft.AspNetCore.Mvc;

//namespace AssetNexIT.API.Controllers.V2;

//[ApiController]
//[ApiVersion("2.0")]
//[Route("api/v{version:apiVersion}/asset-requests")]
//public class AssetRequestsController : ControllerBase
//{
    
//GET https://localhost:5001/api/v1/asset-requests
//V2
//bash
//Copy code
//GET https://localhost:5001/api/v2/asset-requests




//Then just log both in console during demo.

// How to explain to ma’am(say THIS)
//“I’ve implemented URL-based API versioning.
//v1 returns the original response.
//v2 improves the response format without breaking existing clients.
//This allows backward compatibility and safe evolution.”

