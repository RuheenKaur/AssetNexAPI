//using Asp.Versioning;
using Microsoft.IdentityModel.Abstractions;
using AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;


namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/asset-requests")]
    public class AssetRequestController : ControllerBase
    {
        private readonly IAssetsRequestsRep _repo;

        public  AssetRequestController(IAssetsRequestsRep repo)
        {
            _repo = repo;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();

          
            var response = data.Select(x => new
            {
                x.Id,
             
                x.RequestedAssetType,
                x.Status,
                RequestedOn = x.RequestedOn.ToString("dd-MMM-yyyy")
            });

            var res = data.Select(x => new
            {
                x.Id,
                x.RequestedAssetType,
                x.Status,
                RequestedOn = x.RequestedOn
            });
         
            return Ok(response);
        }
    }
}

