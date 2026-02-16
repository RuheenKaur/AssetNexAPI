using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{
    public interface IAssetsHistoryRep
    {
        IEnumerable<AssetsHistory> GetAll();
        AssetsHistory GetById(int id);
        void Create(AssetsHistory history);
        void Update(int id, AssetsHistory history);
        void Delete(int id);
    }
}
