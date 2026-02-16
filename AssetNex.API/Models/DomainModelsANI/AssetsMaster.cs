using Azure.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class AssetsMaster
    {
        public int Id { get; set; }
       
        public int StatusId { get; set; }
        public required string AssetTag { get; set; }
        public required string AssetType { get; set; }
        public required string Brand { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string RAM_GB { get; set; }
        public string Storage_GB { get; set; }
        public int PurchaseCost { get; set; }
        public DateTime WarrantyDate { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int DepartmentId { get; set; }

    }
}

