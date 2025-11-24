

namespace AssetNex.API.Models.DTO.Asset
{
    public class CreateAssetDto
    {

        public required string Name { get; set; }
        public required string SerialNumber { get; set; }
        public required string Department { get; set; }
        public required DateTime DateOfIssue { get; set; }
        public required DateTime WarrantyDate { get; set; }
        public required Guid AssetTypeId { get; set; }
        public required string Status { get; set; }
        public required Guid UserId { get; set; }


    }
}


