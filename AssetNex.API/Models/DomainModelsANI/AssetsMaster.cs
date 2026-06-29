using Azure.Identity;

using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class AssetsMaster
    {
        public int Id { get; set; }
       
        public int StatusId { get; set; }
        public  string AssetTag { get; set; }
        public  string AssetType { get; set; }
        public string Brand { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string RAM_GB { get; set; }
        public string Storage_GB { get; set; }
        public DateTime? CreatedOn { get; set; }

        [JsonIgnore] 
        public StatusMaster Status { get; set; }
        public int PurchaseCost { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int DepartmentId { get; set; }

        public bool IsDeleted { get; set; }

        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
