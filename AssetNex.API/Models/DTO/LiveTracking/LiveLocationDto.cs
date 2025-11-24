namespace AssetNex.API.Models.DTO.LiveTracking
{
    public class LiveLocationDto
    {

        public int Id { get; set; }
        public required string SerialNumber { get; set; }
        public Guid AssetTypeId { get; set; }
        public required string AssetType { get; set; }
        public required string Location { get; set; }
        public required string LastCheckedOut { get; set; }
        public required string Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string Name { get; set; }
    }

}


