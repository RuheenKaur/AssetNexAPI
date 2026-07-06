using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SupportTickets
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int CreatedBy { get; set; }
    public Users User { get; set; }

    [ForeignKey(nameof(Asset))]
    public int AssetId { get; set; }
    public AssetsMaster Asset { get; set; }
    public string IssueCategory { get; set; }
    public string IssueDescription { get; set; }
    public string Priority { get; set; }

    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusMaster Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    public bool IsDeleted { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
}