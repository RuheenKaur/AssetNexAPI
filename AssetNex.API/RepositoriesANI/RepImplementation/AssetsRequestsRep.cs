
using AssetNex.API.Data;
using AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.AssetRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static AssetNex.API.RepositoriesANI.RepImplementation.AssetsRequestsRep;



namespace AssetNex.API.RepositoriesANI.RepImplementation
{
    public class AssetsRequestsRep : IAssetsRequestsRep
    {
        private readonly AppDbContext _context;

        public AssetsRequestsRep(AppDbContext context)
        {
            _context = context;
        }

  
            public async Task CreateAsync(AssetRequests request)
            {
                await _context.AssetRequests.AddAsync(request);
                await _context.SaveChangesAsync();
            }

            public async Task<IEnumerable<AdminAssetRequestDto>> GetAllAsync()
            {
                return await _context.AssetRequests
                    .Include(r => r.User)
                    .Include(r => r.Asset)
                    .Include(r => r.Status)
                    .OrderByDescending(r => r.RequestedOn)
                    .Select(r => new AdminAssetRequestDto
                    {
                        Id = r.Id,
                        Name = r.User.Name,
                        Email = r.User.Email,
                        Contact = r.User.Contact,
                        Asset = r.Asset != null ? r.Asset.AssetTag : "—",
                        RequestedAssetType = r.RequestedAssetType,
                        Reason = r.Reason,
                        Status = r.Status.StatusName,
                        RequestedOn = r.RequestedOn,
                     StatusId   = r.StatusId
                    })
                    .ToListAsync();
            }
        public async Task<AssetRequests> Update(AssetRequests model)
        {
            _context.AssetRequests.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }


        //public async Task<bool> Delete(int id)
        //{
        //    var record = await _context.AssetRequests.FirstOrDefaultAsync(a => a.Id ==id);

        //    if (record == null) return false;
        //    _context.AssetRequests.Remove(record);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> Delete(int id, string deletedBy)
        {
            var record = await _context.AssetRequests.FirstOrDefaultAsync(a => a.Id == id);
            if (record == null) return false;

            record.IsDeleted = true;
            record.DeletedBy = deletedBy;
            record.DeletedOn = DateTime.UtcNow;

            _context.AssetRequests.Update(record);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AssetRequests> Add(AssetRequests request)
        {
            _context.AssetRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<AssetRequests>> GetAll() =>
            await _context.AssetRequests.ToListAsync();

        public async Task<AssetRequests?> Get(int id) =>
            await _context.AssetRequests.FindAsync(id);

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}

       