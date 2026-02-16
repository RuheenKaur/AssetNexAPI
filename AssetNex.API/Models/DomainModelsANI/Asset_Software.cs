namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class Asset_Software
    {
        public int Id { get; set; }
  
        public string Version { get; set; }
        public int StatusId { get; set; }
       
        public int AssetId { get; set; }
        public string License_Key { get; set; }
        public DateTime InstalledAt { get; set; }


    }
}
