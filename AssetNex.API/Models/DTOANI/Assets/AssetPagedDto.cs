using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
    
    namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets
{
    public class AssetPagedDto
    {
        public int Id { get; set; }
        public string AssetTag { get; set; }
        public string AssetType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
       
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string AssignedTo { get; set; }  
    }
}