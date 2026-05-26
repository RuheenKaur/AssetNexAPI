using System.ComponentModel.DataAnnotations.Schema;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class AssetsHistory
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public AssetsMaster Asset { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public int StatusId { get; set; }
        public DateTime? EventDate { get; set; }       
        public DateTime? AssignedDate { get; set; }     
        public DateTime? ReturnedDate { get; set; }     
        public int? ReferenceTicketId { get; set; }     
        public int? CostIncurred { get; set; }          
        public string ModifiedBy { get; set; }
        public StatusMaster Status { get; set; }  
        public string EventType { get; set; }
        public string Remarks { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime PerformedAt { get; set; }
        public string Vendor { get; set; }
       
    }
}
