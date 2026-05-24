using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using System.Collections.Generic;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using System.Threading.Tasks;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{
    public interface IAssetsMasterRep
    {
        Task<IEnumerable<AssetsMaster>> GetAllAsync();
        Task<AssetsMaster?> GetAsync(int userId);
        Task<AssetsMaster?> GetAsyncStatus(int statusId);
        Task<AssetsMaster> AddAsync(AssetsMaster model);
        Task<AssetsMaster> UpdateDetails(AssetsMaster model);
        Task<AssetsMaster> UpdateAsync(AssetsMaster model);
        Task<bool> DeleteAsync(int id);


        Task<PagedResultAssets<AssetPagedDto>> GetAssetsPagedAsync(int page, int pageSize, string search);
    }
}
    