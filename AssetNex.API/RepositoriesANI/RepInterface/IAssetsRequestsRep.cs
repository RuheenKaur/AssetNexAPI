
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.AssetRequests;

namespace AssetNex.API.RepositoriesANI.RepInterface
{
    public interface IAssetsRequestsRep
    {   
        Task<AssetRequests> Update(AssetRequests model);
        Task<bool> Delete(int id);
        Task<AssetRequests> Add(AssetRequests request);

        Task<bool> Delete(int id, string deletedBy);
        Task<IEnumerable<AssetRequests>> GetAll();
        Task CreateAsync(AssetRequests request);
        Task<IEnumerable<AdminAssetRequestDto>> GetAllAsync();
        Task<AssetRequests?> Get(int id);
    }
}
