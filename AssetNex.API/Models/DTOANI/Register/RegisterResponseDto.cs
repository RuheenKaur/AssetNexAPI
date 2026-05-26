namespace AssetNex.API.Models.DTOANI.Register
{
    public class RegisterResponseDto
    {

            public bool Success { get; set; }
            public string Message { get; set; }
            public IEnumerable<string>? Errors { get; set; }

            public  string Token { get; set; }
            public DateTime? Expiration { get; set; }
      
    }

    
}
