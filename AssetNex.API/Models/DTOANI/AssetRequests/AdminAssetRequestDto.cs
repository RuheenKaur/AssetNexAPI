namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.AssetRequests
{

    public class AdminAssetRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Asset { get; set; }
        public string RequestedAssetType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

        public int StatusId { get; set; }
        public DateTime RequestedOn { get; set; }
    }

}

