
using AssetNex.API.Models.DomainModel;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class AssetAssignments

    {   
        public int Id { get; set; }
        public int AssetId  { get; set; }
        public AssetsMaster Asset { get; set; }
        public int UserId { get; set; }
        public DateTime? ReturnedOn { get; set; }
        public string AssetAssigned { get; set; }
        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
        public Users User { get; set; }
    }
}

