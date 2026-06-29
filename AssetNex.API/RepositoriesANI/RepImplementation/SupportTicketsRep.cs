using AssetNex.API.Models.DomainModel;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Dropbox.Api.TeamLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using static Dropbox.Api.Files.SearchMatchType;

public class SupportTicketsRep : ISupportTicketsRep
{
    private readonly AppDbContext _context;

    public SupportTicketsRep(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(SupportTickets ticket)
    {
        ticket.CreatedAt = DateTime.UtcNow;
        _context.SupportTickets.Add(ticket);
        await _context.SaveChangesAsync();
    }




    public async Task<List<SupportTickets>> GetUserTicketsAsync(int createdBy)
    {
        return await _context.SupportTickets
            .Where(t => t.CreatedBy == createdBy)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

   

    public async Task<List<TicketTrackingDto>> GetUserTicketsWithStatus(int userId)
    {
        return await _context.SupportTickets
            .Where(t => t.CreatedBy == userId)
            .Include(t => t.Asset)
            .Include(t => t.Status)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TicketTrackingDto
            {
                TicketId = t.Id,
                IssueCategory = t.IssueCategory,
                IssueDescription = t.IssueDescription,
                Priority = t.Priority,
                AssetConcerned = t.Asset != null ? t.Asset.AssetType : "Unknown",
                StatusName = t.Status != null ? t.Status.StatusName : "Open",
                StatusCategory = t.Status != null ? t.Status.StatusCategory : "",
                ResolutionNotes = t.ResolutionNotes ?? "",
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PagedResult<SupportTicketAdminDto>> GetAdminTicketsPagedAsync(
         int pageNumber,
         int pageSize,
         string? search,
         string? sortField,
         string? sortOrder)
    {
        var query = _context.SupportTickets
            .Include(t => t.User)
            .Include(t => t.Status)
            .Include(t => t.Asset)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(t =>
                (t.IssueCategory != null && t.IssueCategory.ToLower() == (searchLower)) ||
                (t.IssueDescription != null && t.IssueDescription.ToLower().Contains(searchLower)) ||
                (t.User != null && t.User.Name.ToLower().Contains(searchLower)) ||
                (t.Asset != null && t.Asset.AssetType.ToLower().Contains(searchLower))
            );
        }   
        query = sortField switch
        {
            "Name" => sortOrder == "asc"
                ? query.OrderBy(t => t.User.Name)
                : query.OrderByDescending(t => t.User.Name),
            "Priority" => sortOrder == "asc"
                ? query.OrderBy(t => t.Priority)
                : query.OrderByDescending(t => t.Priority),
            "CreatedAt" => sortOrder == "asc"
                ? query.OrderBy(t => t.CreatedAt)
                : query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };


        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new SupportTicketAdminDto
            {
                Id = t.Id,
                Name = t.User != null ? t.User.Name : "Unknown",
                Email = t.User != null ? t.User.Email : "",
                Contact = t.User != null ? t.User.Contact : "",
                AssetConcerned = t.Asset != null ? t.Asset.AssetType : "Unknown",
                IssueCategory = t.IssueCategory,
                IssueDescription = t.IssueDescription,
                Priority = t.Priority,
                CreatedAt = t.CreatedAt,
                 StatusId= t.StatusId,
                Status = t.Status != null ? t.Status.StatusName : "Open",
            })
            .ToListAsync();

        return new PagedResult<SupportTicketAdminDto>
        {
            Data = items,
            TotalCount = totalCount,
            Page = pageNumber,
            PageSize = pageSize
        };
    }
    public async Task UpdateStatusAsync(int ticketId, int statusId)
    {
        var ticket = await _context.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return;
        ticket.StatusId = statusId;
        await _context.SaveChangesAsync();
    }
    public async Task<SupportTickets?> GetByIdAsync(int id)
    {
        return await _context.SupportTickets.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> SoftDeleteAsync(int id, string deletedBy)
    {
        var ticket = await _context.SupportTickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return false;

        ticket.IsDeleted = true;
        ticket.DeletedBy = deletedBy;
        ticket.DeletedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task AddCommentAsync(TicketComment comment)
    {
        comment.CreatedAt = DateTime.UtcNow;
        _context.TicketComments.Add(comment);
        await _context.SaveChangesAsync();

    }

    public async Task<List<TicketComment>> GetCommentsByTicketIdAsync(int ticketId)
    {
        return await _context.TicketComments.Where(c => c.TicketId == ticketId).OrderBy(
            c => c.CreatedAt).AsNoTracking().ToListAsync();
    }

    public async Task<List<object>> GetAssignedAssetsByUserAsync(int createdBy)
    {
        return await _context.SupportTickets
            .Where(t => t.CreatedBy == createdBy)
            .Include(t => t.Asset)
            .Select(t => new
            {
                t.AssetId,
                AssetType = t.Asset != null ? t.Asset.AssetType : "Unknown"
            })
            .Distinct()
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<bool> DeleteAsync(int id, string deletedBy)
    {
        var ticket = await _context.SupportTickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return false;

        ticket.IsDeleted = true;
        ticket.DeletedBy = deletedBy;
        ticket.DeletedOn = DateTime.UtcNow;

        _context.SupportTickets.Update(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

}

