using System.ComponentModel.DataAnnotations.Schema;
using AssetNex.API.Models.DomainModel;

namespace AssetNex.API.Models.DTO.Asset
{
    public class CreateInventoryDto
    {
        public Guid Id { get; set; }
        public required string SerialNumber { get; set; }

        public string Name { get; set; }

        public string Department
        {
            get; set;
        }

        public required AssetType AssetType { get; set; }
        public Guid AssetTypeId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime WarrantyDate { get; set; }

        [ForeignKey("AssetTypeId")]
        public Guid UserId { get; set; }
        public required string User { get; set; }
        public required string Status { get; set; }



    }
}
