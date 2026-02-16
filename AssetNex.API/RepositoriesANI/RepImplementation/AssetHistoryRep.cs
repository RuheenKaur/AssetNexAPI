using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation
{
    public class AssetHistoryRep : IAssetsHistoryRep
    {
        private readonly AppDbContext _context;

        public AssetHistoryRep(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AssetsHistory> GetAll() => _context.AssetHistory.ToList();

        public AssetsHistory GetById(int id) => _context.AssetHistory.Find(id);

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
