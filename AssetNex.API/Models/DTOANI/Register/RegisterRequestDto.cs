namespace AssetNex.API.Models.DTOANI.Register
{
    public class RegisterRequestDto
    {
        public required string Email { get; set; }

        public required string Password { get; set; }    

        public string Name { get; set; }

        public string Contact { get; set; }
    }
    
}
