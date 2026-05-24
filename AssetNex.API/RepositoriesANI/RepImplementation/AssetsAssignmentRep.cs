using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets;
using Microsoft.EntityFrameworkCore;

public class AssetsAssignmentRep : IAssetsAssignmentRep
{
    private readonly AppDbContext _context;

    public AssetsAssignmentRep(AppDbContext context)
    {
        _context = context;
    }

    public async Task AssignAsync(int assetId, int assignedToUserId, int assignedByUserId)
    {
     
        var asset = await _context.AssetMaster.FindAsync(assetId);
        if (asset == null)
            throw new Exception("Asset not found");

      
        var activeAssignment = await _context.AssetAssignments
            .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedOn == null);

        if (activeAssignment != null)
            throw new Exception("Asset is already assigned");

        
        var assignment = new AssetAssignments
        {
            AssetId = assetId,
            UserId = assignedToUserId,
            AssetAssigned = asset.AssetType,
            AssignedOn = DateTime.Now,
          
        };

        await _context.AssetAssignments.AddAsync(assignment);

      
        var history = new AssetsHistory
        {
            AssetId = assetId,
            UserId = assignedByUserId,
            EventType = "Assigned",
            EventDate = DateTime.Now,
            Remarks = $"Assigned to UserId {assignedToUserId}"
        };

        await _context.AssetHistory.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task ReturnAsync(int assetId, int returnedByUserId, string remarks)
    {
        var assignment = await _context.AssetAssignments
            .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedOn == null);

        if (assignment == null)
            throw new Exception("No active assignment found");

        assignment.ReturnedOn = DateTime.Now;

        await _context.AssetHistory.AddAsync(new AssetsHistory
        {
            AssetId = assetId,
            UserId = returnedByUserId,
            EventType = "Returned",
            EventDate = DateTime.Now,
            Remarks = remarks
        });

        await _context.SaveChangesAsync();
    }



    public async Task<IEnumerable<AssetsHistory>> GetHistory(int assetId)
    {
        return await _context.AssetHistory
            .Where(h => h.AssetId == assetId)
            .OrderByDescending(h => h.EventDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<AssetAssignments>> GetAll()
        => await _context.AssetAssignments.ToListAsync();

    public async Task<IEnumerable<AssignedAssetDto>> GetAssignedAssetsByUserId(int userId)
    {
        return await _context.AssetAssignments
            .Where(a => a.UserId == userId)
            .Join(
                _context.AssetMaster,
                a => a.AssetId,
                asset => asset.Id,
                (a, asset) => new AssignedAssetDto
                {
                    AssetId = asset.Id,
                    AssetTag = asset.AssetTag,
                    AssetType = asset.AssetType,
                    Brand = asset.Brand,
                    Model = asset.Model,
                    SerialNumber = asset.SerialNumber,
                    AssignedOn = a.AssignedOn,
                    UserId = a.UserId
                }
            )
            .ToListAsync();
    }


    public async Task<IEnumerable<AssignedAssetDto>> GetAssignedAssetsByUserIdd(int userId)

    {
        return await _context.AssetAssignments
            .Where(a => a.UserId == userId).Join(_context.AssetMaster,
            a => a.AssetId,
            asset => asset.Id,
            (a, asset) => new AssignedAssetDto
            {
                AssetId = asset.Id,
                AssetTag = asset.AssetTag,
                AssetType = asset.AssetType,
                Brand = asset.Brand,
                Model = asset.Model,
                SerialNumber = asset.SerialNumber,
                AssignedOn = a.AssignedOn,
                UserId = a.UserId
            }
            ).ToListAsync();

    }


}
