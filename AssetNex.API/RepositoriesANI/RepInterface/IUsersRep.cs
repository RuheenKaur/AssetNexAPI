using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{
    public interface IUsersRep
    {
        IEnumerable<Users> GetAll();
        Users GetById(int id);
        Task<IEnumerable<Users>> GetByUserEmailAsync(string email);
     
        void Create(Users user);
        void Update(int id, Users user);
        void Delete(int id);
        Task<Users?> GetUserByIdAsync(int id);
        Task<UserProfileDto?> GetUserProfileAsync(int id);
        Task<Users?> GetByEmailAsync(string email);
        Task<Users> UpdateAsync(Users user);
        Task<bool> DeactivateUserAsync(int userId);
        
    }
}

 
