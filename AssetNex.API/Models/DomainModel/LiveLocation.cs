using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace AssetNex.API.Models.DomainModel
{
    public class LiveLocation
    {
        public int Id { get; set; }
        [ForeignKey("AssetInfo")]
        public required string SerialNumber { get; set; }
        [ForeignKey("AssetInfo")]
        public required Guid AssetTypeId { get; set; }
        [ForeignKey("AssetInfo")]
        public required string AssetType { get; set; }
        [ForeignKey("AssetInfo")]
        public required string Status { get; set; }
        public required string Location { get; set; }
        public required string LastCheckedOut { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string Name { get; set; }


    }

}

