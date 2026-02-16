namespace AssetNex.API.Models.DTOANI.Support
{
    public class SupportTicketDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int  Contact { get; set; }

        public int AssignedToUserId { get; set; }
        public string IssueCategory { get; set; }
        public string AssetConcerned { get; set; }
        public string IssueDescription { get; set; }
        public string Priority { get; set; }
    }
}
