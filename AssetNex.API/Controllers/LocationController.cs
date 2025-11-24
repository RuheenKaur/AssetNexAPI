using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Models.DTO.LiveTracking;
using AssetNex.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNex.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class LocationController : ControllerBase

    {
        private readonly ILocationRepository locationRepository;

        private readonly ApplicationDbContext dbContext;

        public LocationController(ILocationRepository locationRepository, ApplicationDbContext dbContext)
        {
            this.locationRepository = locationRepository;
            this.dbContext = dbContext;

        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetAllLocations()
        {
            var assets = await locationRepository.GetAllLocationAsync();
            var response = assets.Select(asset => new LiveLocationDto

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

            })
            .ToList();
            return Ok(response);
        }
        [HttpPost("locations/create")]
        public IActionResult CreateLocation(LiveLocationDto dto)
        {
            var asset = new LiveLocation
            {
                Id = dto.Id,
                SerialNumber = dto.SerialNumber,
                AssetTypeId = dto.AssetTypeId,
                AssetType = dto.AssetType,
                Status = dto.Status,
                Location = dto.Location,
                LastCheckedOut = dto.LastCheckedOut,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Name = dto.Name,
            };

            dbContext.Location.Add(asset);
            dbContext.SaveChanges();
            return Ok(asset);
        }
    }

}






