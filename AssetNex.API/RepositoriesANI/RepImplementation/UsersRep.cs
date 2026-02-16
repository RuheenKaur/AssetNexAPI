using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.EntityFrameworkCore;
using static AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation.UsersRep;
using static Dropbox.Api.Files.ListRevisionsMode;
using static Dropbox.Api.Files.SearchMatchType;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation
{
    public class UsersRep : IUsersRep
    {
        private readonly AppDbContext _context;

        public UsersRep(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Users>> GetByUserEmailAsync(string email)
        {

            return await _context.Users
                .Where(u => u.Email == email)
                .ToListAsync();
        }

        public IEnumerable<Users> GetAll() => _context.Users.ToList();
        public Users GetById(int id) => _context.Users.Find(id);

        public void Create(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(int id, Users user)
        {
            var existing = _context.Users.Find(id);
            if (existing == null) return;

            _context.Entry(existing).CurrentValues.SetValues(user);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return;

            _context.Users.Remove(user);
            _context.SaveChanges();
        }


        public async Task<Users?> GetUserByIdAsync(int id)

        {
            return await _context.Users
           .Where(u => u.Id == id)
           .FirstOrDefaultAsync();

        }

        public async Task<UserProfileDto?> GetUserProfileAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserProfileDto
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

   

        public async Task<Users> UpdateAsync(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
