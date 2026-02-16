using AssetNex.API.Data;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket
{
    public class TicketResponseDto
    {
        public string TicketStatus { get; set; }
        public int Id { get; set; } 
        public string IssueCategory { get; set; }
        public string IssueDescription { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string ResolutionNotes { get; set; }
        public int AssignedToUserId { get; set; }

    }
}
