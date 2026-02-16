namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.AssetRequests
{

    public class AssetRequestDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Contact { get; set; }
        public string RequestedAssetType { get; set; }
        public string Reason { get; set; }
    }
}
