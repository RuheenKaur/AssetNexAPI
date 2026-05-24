using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{
    public interface IUsersRep
    {
        Task<IEnumerable<Users>> GetAllAsync();

        Task<Users?> GetByIdAsync(int id);

        Task<Users?> GetByEmailAsync(string email);

        Task CreateAsync(Users user);

        Task UpdateAsync(Users user);

        Task DeleteAsync(int id);

        Task<bool> DeactivateUserAsync(int userId);
    }
}