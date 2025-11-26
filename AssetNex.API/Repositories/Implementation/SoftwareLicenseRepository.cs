
using AssetNex.API.Data;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AssetNex.API.Repositories.Implementation
{
    public class SoftwareLicenseRepository : ISoftwareLicenseRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SoftwareLicenseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SoftwareLicenseInfo?> CreateAsync(SoftwareLicenseInfo license)

        {
            await dbContext.SoftwareLicenseInfo.AddAsync(license);

            await dbContext.SaveChangesAsync();

            return license;
        }



        public async Task<List<SoftwareLicenseInfo>> GetAllSoftwareLicense()
        {
            return await dbContext.SoftwareLicenseInfo.ToListAsync();

        }

        public async Task<IEnumerable<SoftwareLicenseInfo>> GetSoftwareLicenseAsync()
        {
            return await dbContext.SoftwareLicenseInfo
                .Select(a => new SoftwareLicenseInfo
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    Request = a.Request,
                    EmployeeId = a.EmployeeId,
                    SoftwareName = a.SoftwareName,
                    OtherSoftware = a.OtherSoftware,
                    DateApplied = a.DateApplied,
                })
                .ToListAsync();
        }



        public async Task<SoftwareLicenseInfo?> GetBySoftwareLicenseIdAsync(Guid id)
        {

            return await dbContext.SoftwareLicenseInfo.FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<SoftwareLicenseInfo?> UpdateSoftwareLicenseAsync(SoftwareLicenseInfo softwareLicense)
        {
            var existingSoftwareLicense = await dbContext.SoftwareLicenseInfo.FirstOrDefaultAsync(x => x.Id == softwareLicense.Id);

            if (existingSoftwareLicense != null)
            {
                dbContext.Entry(existingSoftwareLicense).CurrentValues.SetValues(softwareLicense);
                await dbContext.SaveChangesAsync();
                return softwareLicense;
            }

            return null;
        }

        public async Task<SoftwareLicenseInfo?> DeleteAsync(Guid id)
        {
            var existingSoftwareLicense = await dbContext.SoftwareLicenseInfo.FirstOrDefaultAsync(x => x.Id == id);

            if (existingSoftwareLicense != null)
            {
                return null;

            }

            dbContext.SoftwareLicenseInfo.Remove(existingSoftwareLicense!);
            await dbContext.SaveChangesAsync();
            return existingSoftwareLicense;
        }




    }


}
