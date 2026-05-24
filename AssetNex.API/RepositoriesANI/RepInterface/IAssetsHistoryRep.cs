using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{


    public interface IAssetsHistoryRep
    {
        Task<IEnumerable<AssetHistoryDto>> GetAllAsync();
        Task<IEnumerable<AssetHistoryDto>> GetByAssetIdAsync(int assetId);
        Task CreateAsync(AssetsHistory history);
        IEnumerable<AssetsHistory> GetAll();
        AssetsHistory GetById(int id);
        void Create(AssetsHistory history);
        void Update(int id, AssetsHistory history);
        void Delete(int id);
    }

}