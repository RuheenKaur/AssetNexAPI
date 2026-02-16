namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

}

