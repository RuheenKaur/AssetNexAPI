using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetSoftwareController : ControllerBase
    {
        private readonly IAssetSoftwareRep _repo;

        public AssetSoftwareController(IAssetSoftwareRep repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{softwareId}")]
        public IActionResult Get(int softwareId) => Ok(_repo.GetById(softwareId));

        [HttpPost]
        public IActionResult Create(Asset_Software software)
        {
            _repo.Create(software);
            return Ok("Software Added Successfully");
        }

        [HttpPut("{softwareId}")]
        public IActionResult Update(int softwareId, Asset_Software software)
        {
            _repo.Update(softwareId, software);
            return Ok("Software Updated Successfully");
        }

        [HttpDelete("{softwareId}")]
        public IActionResult Delete(int softwareId)
        {
            _repo.Delete(softwareId);
            return Ok("Software Deleted Successfully");
        }
    }
}
