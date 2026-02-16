using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{
    public interface IAssetSoftwareRep
    {
        IEnumerable<Asset_Software> GetAll();
        Asset_Software GetById(int softwareId);
        void Create(Asset_Software software);
        void Update(int softwareId, Asset_Software software);
        void Delete(int softwareId);
    }
}
