namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets
{
    public class AssetHistoryDto
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string AssetTag { get; set; }
        public string AssetType { get; set; }
        public string ModifiedBy { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EventType { get; set; }
        public string Remarks { get; set; }
        public DateTime PerformedAt { get; set; }
     
    }
}