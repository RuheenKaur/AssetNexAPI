using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalAssets = await _context.AssetMaster.CountAsync();


            var availableAssets = await _context.AssetMaster
           .Where(a => a.StatusId == 10)
            .CountAsync();

            
            var assetsInUse = await _context.AssetMaster
                .Where(a => a.StatusId == 9)
                .CountAsync();

            var pendingTickets = await _context.SupportTickets
            .Where(t => t.StatusId == 1 || t.StatusId == 2)
            .CountAsync();

             var pendingRequests = await _context.AssetRequests
            .Where(r => r.StatusId == 11)
            .CountAsync();

            var approvedRequests = await _context.AssetRequests
                .Where(r => r.StatusId == 12)
                .CountAsync();

            var rejectedRequests = await _context.AssetRequests
                .Where(r => r.StatusId == 13)
                .CountAsync();


            var openTickets = await _context.SupportTickets
                .Where(t => t.StatusId == 1)
                .CountAsync();

            var inProgressTickets = await _context.SupportTickets
                .Where(t => t.StatusId == 2)
                .CountAsync();

            var resolvedTickets = await _context.SupportTickets
                .Where(t => t.StatusId == 3)
                .CountAsync();

            var totalUsers = await _context.Users
                .Where(u => u.IsActive)
                .CountAsync();

            var assetsByStatus = await _context.AssetMaster
                .GroupBy(a => a.StatusId)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(new
            {
                totalAssets,
                assetsInUse,
                availableAssets,
                pendingTickets,
                totalUsers,
                tickets = new
                {
                    open = openTickets,
                    inProgress = inProgressTickets,
                    resolved = resolvedTickets
                },
                requests = new
                {
                    pending = pendingRequests,
                    approved = approvedRequests,
                    rejected = rejectedRequests
                },
                assetsByStatus
            });
        }
    }
}


