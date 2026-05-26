namespace AssetNex.API.Models.DomainModelsANI
{
    public class RefreshTokenModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsExpired => DateTime.UtcNow >= Expiry;
        public DateTime? Expiry { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

    }
}
