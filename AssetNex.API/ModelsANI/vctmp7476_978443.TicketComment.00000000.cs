namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ModelsANI
{
    public class TicketComment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Comment { get; set; }

        public string Type { get; set; }

        public int CommentedByUserId { get; set; }

        //    public string Type { get; set; } =
        //"Internal";

        //public bool isPublic { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
