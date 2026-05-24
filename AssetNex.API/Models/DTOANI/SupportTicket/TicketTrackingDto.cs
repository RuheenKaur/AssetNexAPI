
public class TicketTrackingDto
{
    public int TicketId { get; set; }
    public string IssueCategory { get; set; }
    public string IssueDescription { get; set; }
    public string Priority { get; set; }
    public string AssetConcerned { get; set; }
    public string StatusName { get; set; }
    public string StatusCategory { get; set; }
    public string ResolutionNotes { get; set; }
    public DateTime CreatedAt { get; set; }
}