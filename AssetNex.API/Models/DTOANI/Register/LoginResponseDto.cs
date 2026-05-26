namespace AssetNex.API.Models.DTO.Register
{
    public class LoginResponseDto
    {

            public required string Token { get; set; }
            public DateTime Expiration { get; set; }
    }

}


