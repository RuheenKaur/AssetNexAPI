using System.ComponentModel.DataAnnotations.Schema;

namespace AssetNex.API.Models.DomainModel
{
    public class AssetLocations
    {


        public int Id { get; set; }

        [ForeignKey("AssetInfo")]

        public required string SerialNumber { get; set; }

        [ForeignKey("AssetInfo")]

        public Guid AssetTypeId { get; set; }


        [ForeignKey("AssetInfo")]

        public required string AssetType { get; set; }

        [ForeignKey("AssetInfo")]
        public required string Status { get; set; }

        public required string Location { get; set; }

        public required string LastCheckedOut { get; set; }

        public string CheckedOutBy { get; set; }

        public string CheckedOutByEmail { get; set; }

        public string DateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string Name { get; set; }

    }
}
