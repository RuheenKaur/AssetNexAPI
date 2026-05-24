using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.EntityFrameworkCore;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication;


namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation
{
    public class AssetsMasterRep : IAssetsMasterRep
    {
        private readonly AppDbContext _context;

        public AssetsMasterRep(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetsMaster>> GetAllAsync()
        {
            return await _context.AssetMaster
                                 .AsNoTracking()
                                 .ToListAsync();
        }

 
        public async Task<AssetsMaster?> GetAsync(int userId)
        {
            return await _context.AssetMaster
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<AssetsMaster?> GetAsyncStatus(int statusId)
        {
            return await _context.AssetMaster
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(a => a.Id == statusId);
        }
  
        public async Task<AssetsMaster?> GetAsyncStatus(int statusId, int userId)
        {
            return await _context.AssetMaster.AsNoTracking().FirstOrDefaultAsync(a => a.Id == statusId);
        }

      
        public async Task<PagedResultAssets<AssetPagedDto>> GetAssetsPagedAsync(int page, int pageSize, string search)
        {
            var query = _context.AssetMaster
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x =>
                    (x.AssetTag != null && x.AssetTag.ToLower().Contains(s)) ||
                    (x.Brand != null && x.Brand.ToLower().Contains(s)) ||
                    (x.AssetType != null && x.AssetType.ToLower().Contains(s))
                );
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new AssetPagedDto
                {
                    Id = t.Id,
                    AssetTag = t.AssetTag,
                    AssetType = t.AssetType,
                    Brand = t.Brand,
                    Model = t.Model,
                    SerialNumber = t.SerialNumber,
                    StatusId = t.StatusId,
                    StatusName = t.Status != null ? t.Status.StatusName : "—",
                    AssignedTo = _context.AssetAssignments
    .Where(a => a.AssetId == t.Id && a.ReturnedOn == null)
    .Join(_context.Users,
        a => a.UserId,
        u => u.Id,
        (a, u) => u.Name)
    .FirstOrDefault()
                })
                .ToListAsync();

            return new PagedResultAssets<AssetPagedDto>
            {
                Data = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<AssetsMaster> AddAsync(AssetsMaster model)
        {
            var entry = (await _context.AssetMaster.AddAsync(model)).Entity;
            await _context.SaveChangesAsync();
            return entry;

        }
        public async Task<AssetsMaster> UpdateAsync(AssetsMaster model)
        {
            var existing = await _context.AssetMaster.FirstOrDefaultAsync(a => a.Id == model.Id);
            if (existing == null) throw new KeyNotFoundException($"Asset with id {model.Id} not found.");


            existing.AssetTag = model.AssetTag ?? existing.AssetTag;
            existing.AssetType = model.AssetType ?? existing.AssetType;
            existing.Brand = model.Brand ?? existing.Brand;
            existing.Model = model.Model ?? existing.Model;
            existing.RAM_GB = model.RAM_GB ?? existing.RAM_GB;

            existing.SerialNumber = model.SerialNumber;
            existing.Storage_GB = model.Storage_GB;

            existing.DepartmentId = model.DepartmentId;
            existing.PurchaseCost = model.PurchaseCost;
            existing.PurchaseDate = model.PurchaseDate;

            _context.AssetMaster.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<AssetsMaster> UpdateDetails(AssetsMaster model)
        {
            var existing = await _context.AssetMaster.FirstOrDefaultAsync(a => a.Id == model.Id);
            if (existing == null) throw new KeyNotFoundException($"Asset with id {model.Id} not found");

            existing.Brand = model.Brand ?? existing.Brand;
            existing.WarrantyDate = model.WarrantyDate;
            existing.Model = model.Model;
            _context.AssetMaster.Update(existing);
            await _context.SaveChangesAsync();
            return existing;

        }
        public async Task<bool> DeleteAsync(int id)

        { 
            var existing = await _context.AssetMaster.FirstOrDefaultAsync(a => a.Id == id) ?? null; 
            if (existing == null) return false;
            _context.AssetMaster.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
