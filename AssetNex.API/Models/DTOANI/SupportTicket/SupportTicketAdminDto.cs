using  AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket

{   public class SupportTicketAdminDto
    {
        public int Id { get; set; }
        public Users User { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string AssetConcerned { get; set; }
        public string IssueCategory { get; set; }
        public string IssueDescription { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
