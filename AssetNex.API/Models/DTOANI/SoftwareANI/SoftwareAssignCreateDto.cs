using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SoftwareANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SoftwareANI
{
    public class SoftwareAssignCreateDto
    {
        public int UserId { get; set; }
        public int SoftwareId { get; set; } 
       
        public string License_Key { get; set; }
    }
}

