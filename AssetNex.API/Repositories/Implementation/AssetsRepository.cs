using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;


namespace AssetNex.API.Repositories.Implementation
{
    public class AssetsRepository : IAssetsRepository

    {
        private readonly ApplicationDbContext dbContext;

        public AssetsRepository(ApplicationDbContext dbContext)

        {
            this.dbContext = dbContext;

        }
        public async Task<List<AssetInfo>> getAllAssets()
        {
            return await dbContext.AssetInfo.Include(a => a.AssetType).ToListAsync();
        }
        public async Task AddAssetAsync(AssetInfo asset)
        {
            await dbContext.AssetInfo.AddAsync(asset);
            await dbContext.SaveChangesAsync();
        }

        public async Task<AssetInfo?> GetAssetByIdAsync(Guid id)
        {
            return await dbContext.AssetInfo
                .Include(a => a.AssetType)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AssetType?> GetAssetTypeByIdAsync(Guid id)
        {
            return await dbContext.AssetTypes.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<AssetInfo?> UpdateAsync(AssetInfo asset)
        {
            var existingAsset = await dbContext.AssetInfo.FirstOrDefaultAsync(x => x.Id == asset.Id);

            if (existingAsset != null)
            {
                dbContext.Entry(existingAsset).CurrentValues.SetValues(asset);
                await dbContext.SaveChangesAsync();
                return asset;

            }
            return null;
        }

        public async Task<AssetInfo?> GetById(Guid id)
        {
            return await dbContext.AssetInfo.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingAsset = await dbContext.AssetInfo.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAsset == null)
            {
                return false;
            }

            dbContext.AssetInfo.Remove(existingAsset);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}


