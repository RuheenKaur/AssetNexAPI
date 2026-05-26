using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.EntityFrameworkCore;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation
{
    public class UsersRep : IUsersRep
    {
        private readonly AppDbContext _context;

        public UsersRep(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<Users?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

     
        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        
        public async Task CreateAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

       
        public async Task UpdateAsync(Users user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return;

            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.Contact = user.Contact;

            await _context.SaveChangesAsync();
        }

       
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
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