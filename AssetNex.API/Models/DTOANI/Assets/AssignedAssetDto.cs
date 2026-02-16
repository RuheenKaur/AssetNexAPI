using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets
{ 
public class AssignedAssetDto
    {
       
        public int AssetId { get; set; }
        public string AssetTag { get; set; }
        public string AssetType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime AssignedOn { get; set; }
        public int UserId { get; set; }
    }
}