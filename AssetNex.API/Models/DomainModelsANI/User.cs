namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI
{
    public class Users
    {
        public int Id { get; set; }    
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Role { get; set; }  
        public string Contact { get; set; }
        public int DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public DateTime createdOn { get; set; }
    }
}
    