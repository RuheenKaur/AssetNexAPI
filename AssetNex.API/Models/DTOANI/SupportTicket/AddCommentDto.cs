namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket
{
    public class AddCommentDto
    {
        public int TicketId { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; } = "Internal";
        public int CommentedByUserId { get; set; }
    }
}
