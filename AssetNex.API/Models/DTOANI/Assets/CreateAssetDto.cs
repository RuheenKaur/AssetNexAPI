public class CreateAssetDto
{
    public string AssetTag { get; set; }
    public string AssetType { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string RAM_GB { get; set; }

    public DateTime createdOn { get; set; }
    public string Storage_GB { get; set; }
    public int StatusId { get; set; }
    public int PurchaseCost { get; set; }
    public DateTime? WarrantyDate { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public int DepartmentId { get; set; }
}