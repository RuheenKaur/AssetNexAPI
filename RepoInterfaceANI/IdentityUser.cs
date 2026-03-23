namespace Microsoft.AspNet.Identity
{
    public class IdentityUser
    {

        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}









