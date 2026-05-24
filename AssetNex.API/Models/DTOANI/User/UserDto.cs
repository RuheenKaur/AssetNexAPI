using Dropbox.Api.Contacts.Routes;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.User
{
    public class UserDto
    {
         public string Name { get; set; }

        public string Email { get; set; }

        public string Contact { get; set; } 

        public int Id { get; set; }

        public string Role { get; set; } = "User";

        public DateTime? CreatedAt { get; set; }


    }
}
