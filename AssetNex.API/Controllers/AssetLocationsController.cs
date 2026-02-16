
using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.LiveTracking;

using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNex.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class AssetLocationsController : ControllerBase

    {
        private readonly ILocationRepository locationRepository;
        private readonly ApplicationDbContext dbContext;
        public AssetLocationsController(ILocationRepository locationRepository, ApplicationDbContext dbContext)
        {
            this.locationRepository = locationRepository;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssetLocations()
        {
            var assets = await locationRepository.GetAssetLocationAsync();

            var response = assets.Select(asset => new AssetLocations
            {

                Id = asset.Id,
                SerialNumber = asset.SerialNumber,
                AssetTypeId = asset.AssetTypeId,
                AssetType = asset.AssetType,
                Status = asset.Status,
                Location = asset.Location,
                LastCheckedOut = asset.LastCheckedOut,
                Latitude = asset.Latitude,
                Longitude = asset.Longitude,
                Name = asset.Name,


            }).ToList();

            return Ok(response);
        }


        [HttpPost("assetlocations/create")]
        public IActionResult CreateAssetLocation(AssetLocationDto dto)
        {
            var asset = new AssetLocations
            {
                Id = dto.Id,
                SerialNumber = dto.SerialNumber,
                AssetTypeId = dto.AssetTypeId,
                AssetType = dto.AssetType,
                Status = dto.status,
                Location = dto.Location,
                LastCheckedOut = dto.LastCheckedOut,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Name = dto.Name,
            };

            dbContext.AssetLocations.Add(asset);
            dbContext.SaveChanges();
            return Ok(asset);
        }
    }

}

