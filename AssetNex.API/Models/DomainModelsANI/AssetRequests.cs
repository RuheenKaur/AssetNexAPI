using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class AssetRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? AdminNotes { get; set; }        // ← was string

        public int AssetId { get; set; }
        public AssetsMaster? Asset { get; set; }

        public int UserId { get; set; }
        public Users? User { get; set; }

        public int? RequestedBy { get; set; }
        public string? RequestedAssetType { get; set; }

        public string? Reason { get; set; }            // ← was string

        public int StatusId { get; set; }
        public StatusMaster? Status { get; set; }      // ← was StatusMaster (navigation property)

        public DateTime RequestedOn { get; set; }
    }

}
