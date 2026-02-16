namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket
{
    public class AddCommentBodyDto
    {
        public string Message { get; set; }
        public string Type { get; set; }  // "Internal" or "User"
    }
}
