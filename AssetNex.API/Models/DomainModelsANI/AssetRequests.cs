namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class AssetRequests
    {
       public int Id { get; set; }        
        public int AssetId  { get; set; }
        public AssetsMaster Asset { get; set; }
       public int  UserId { get; set; }
       public Users User { get; set; }
       public string RequestedAssetType { get; set; }
       public string Reason { get; set; }
       public int StatusId {  get; set; } 
       public StatusMaster Status {  get; set; }
       public DateTime RequestedOn { get; set; }



    }
}
