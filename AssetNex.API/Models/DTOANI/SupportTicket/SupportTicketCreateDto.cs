public class SupportTicketCreateDto
{
    
    public int UserId { get; set; }
    public int AssetId { get; set; }
    public string IssueCategory { get; set; }
    public string ResolutionNotes { get; set; }
    public string IssueDescription { get; set; }
    public string Priority { get; set; }
}
