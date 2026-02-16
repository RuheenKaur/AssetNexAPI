using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation
{
    public class AssetSoftwareRep : IAssetSoftwareRep
    {
        private readonly AppDbContext _context;

        public AssetSoftwareRep(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Asset_Software> GetAll() => _context.Asset_Software.ToList();

        public Asset_Software GetById(int softwareId) => _context.Asset_Software.Find(softwareId);

        public void Create(Asset_Software software)
        {
            software.InstalledAt = DateTime.Now;
            _context.Asset_Software.Add(software);
            _context.SaveChanges();
        }

        public void Update(int softwareId, Asset_Software software)
        {
            var existing = _context.Asset_Software.Find(softwareId);
            if (existing == null) return;

            _context.Entry(existing).CurrentValues.SetValues(software);
            _context.SaveChanges();
        }


        public void Delete(int softwareId)
        {
            var existing = _context.Asset_Software.Find(softwareId);
            if (existing == null) return;

            _context.Asset_Software.Remove(existing);
            _context.SaveChanges();
        }
    }
}
