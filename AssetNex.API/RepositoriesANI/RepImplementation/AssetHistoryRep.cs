using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.EntityFrameworkCore;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation
{
    public class AssetHistoryRep : IAssetsHistoryRep
    {
        private readonly AppDbContext _context;

        public AssetHistoryRep(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetHistoryDto>> GetAllAsync()
        { 
            return await _context.AssetHistory
                .AsNoTracking()
                .Include(h => h.Asset)
                .Include(h => h.User)
                .OrderByDescending(h => h.PerformedAt)
                .Select(h => new AssetHistoryDto
                {
                    Id = h.Id,
                    AssetId = h.AssetId,
                    AssetTag = h.Asset != null ? h.Asset.AssetTag : "—",
                    AssetType = h.Asset != null ? h.Asset.AssetType : "—",
                    UserId = h.UserId,
                    UserName = h.User != null ? h.User.Name : "—",
                    EventType = h.EventType,
                    Remarks = h.Remarks ?? "—",
                    PerformedAt = h.PerformedAt,
                    ModifiedBy = h.User != null ? h.User.Name : "System"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetHistoryDto>> GetByAssetIdAsync(int assetId)
        {
            return await _context.AssetHistory
                .AsNoTracking()
                .Include(h => h.Asset)
                .Include(h => h.User)
                .Where(h => h.AssetId == assetId)
                .OrderByDescending(h => h.PerformedAt)

                .Select(h => new AssetHistoryDto
                {
                    Id = h.Id,
                    AssetId = h.AssetId,
                    AssetTag = h.Asset != null ? h.Asset.AssetTag : "—",
                    AssetType = h.Asset != null ? h.Asset.AssetType : "—",
                    UserId = h.UserId,
                    UserName = h.User != null ? h.User.Name : "—",
                    EventType = h.EventType,
                    Remarks = h.Remarks ?? "—",
                    PerformedAt = h.PerformedAt,
                    ModifiedBy = !string.IsNullOrEmpty(h.ModifiedBy) ? h.ModifiedBy : "Admin"
                  
                })
                .ToListAsync();
        }

        public async Task CreateAsync(AssetsHistory history)
        {
            
            await _context.AssetHistory.AddAsync(history);
            await _context.SaveChangesAsync();
        }

     
        public IEnumerable<AssetsHistory> GetAll() =>
            _context.AssetHistory.ToList();

        public AssetsHistory GetById(int id) =>
            _context.AssetHistory.Find(id);

        public void Create(AssetsHistory history)
        {
            _context.AssetHistory.Add(history);
            _context.SaveChanges();
        }

        public void Update(int id, AssetsHistory history)
        {
            var existing = _context.AssetHistory.Find(id);
            if (existing == null) return;
            _context.Entry(existing).CurrentValues.SetValues(history);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.AssetHistory.Find(id);
            if (existing == null) return;
            _context.AssetHistory.Remove(existing);
            _context.SaveChanges();
        }
    }
}