namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket
{
    public class TicketCommentDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Type { get; set; } = "Internal";

    }
}
