namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class Warranty
    {
        public int Id { get; set; } 
        public int WarrantyId { get; set; }
        public int AssetId { get; set; }
        public AssetsMaster Asset { get; set; }
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
