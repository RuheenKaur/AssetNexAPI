namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets
{

        public class PagedResultAssets<AssetsMaster>
        {
            public IEnumerable<AssetsMaster> Data { get; set; } = new List<AssetsMaster>();
            public int TotalCount { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        }
}

