using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Mvc;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsHistoryController : ControllerBase
    {
        private readonly IAssetsHistoryRep _repo;

        public AssetsHistoryController(IAssetsHistoryRep repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("asset/{assetId}")]
        public async Task<IActionResult> GetByAsset(int assetId)
        {
            var result = await _repo.GetByAssetIdAsync(assetId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_repo.GetById(id));

        [HttpPost]
        public IActionResult Create(AssetsHistory history)
        {
            _repo.Create(history);
            return Ok("History Created");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, AssetsHistory history)
        {
            _repo.Update(id, history);
            return Ok("History Updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return Ok("History Deleted");
        }

    }
}
