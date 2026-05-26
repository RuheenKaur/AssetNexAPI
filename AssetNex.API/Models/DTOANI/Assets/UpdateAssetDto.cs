namespace AssetNex.API.Models.DTOANI.Assets
{
    public class UpdateAssetDto
    {

        public string AssetTag { get; set; }
        public string AssetType { get; set; }
        public string Brand { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public int StatusId { get; set; }
    }

}



   