using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
public interface IAssetsAssignmentRep
{
    Task<IEnumerable<AssetAssignments>> GetAll();
    Task AssignAsync(int assetId, int assignedToUserId, int assignedByUserId);
    Task ReturnAsync(int assetId, int returnedByUserId, string remarks);
    Task<IEnumerable<AssetsHistory>> GetHistory(int assetId);
    Task<IEnumerable<AssignedAssetDto>> GetAssignedAssetsByUserId(int userId);
  
}
