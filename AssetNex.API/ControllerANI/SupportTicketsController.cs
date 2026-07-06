using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/support-tickets")]
[ApiController]

public class SupportTicketsController : ControllerBase
{
    private readonly ISupportTicketsRep _repo;
    private readonly ISupportTicketService _ticketService;
    private readonly AppDbContext _context;

    public SupportTicketsController(
        ISupportTicketsRep repo,
        ISupportTicketService ticketService,
        AppDbContext context)
    {
        _repo = repo;
        _ticketService = ticketService;
        _context = context;

    }


    [HttpGet("admin")]
    public async Task<IActionResult> GetAdminTickets(
        int pageNumber = 1,
        int pageSize = 10,
        string? search = "",
        string? sortField = "CreatedAt",
        string? sortOrder = "desc")
    {
        var result = await _ticketService.GetTicketsAsync(
            pageNumber,
            pageSize,
            search,
            sortField,
            sortOrder
        );
        return Ok(result);
    }


    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetTicketsByUser(int userId)
    {
        var tickets = await _repo.GetUserTicketsWithStatus(userId);
        return Ok(tickets);
    }


    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketDto dto)
    {
        if (dto.AssetId <= 0 || dto.UserId <= 0)
            return BadRequest("Invalid user or asset");

        var ticket = new SupportTickets
        {
            CreatedBy = dto.UserId,
            AssetId = dto.AssetId,
            IssueCategory = dto.IssueCategory,
            IssueDescription = dto.IssueDescription,
            Priority = dto.Priority,
            StatusId = 6,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.CreateAsync(ticket);

        return Ok(new
        {
            message = "Ticket created successfully",
            ticketId = ticket.Id
        });
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var ticket = await _context.SupportTickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            return NotFound($"Ticket {id} not found");

        await _context.SupportTickets
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.StatusId, dto.StatusId));

        return Ok(new { message = "Status updated", statusId = dto.StatusId });
    }

    [HttpPatch("{id}/resolution")]
    public async Task<IActionResult> UpdateResolutionNotes(
    int id,
    [FromBody] UpdateResolutionDto dto)
    {
        await _context.SupportTickets
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.ResolutionNotes, dto.ResolutionNotes));

        return Ok(new { message = "Resolution notes updated" });
    }

    [HttpPost("{ticketId}/comment")]
    public async Task<IActionResult> AddCommentToTicket(
    int ticketId,
    [FromBody] AddCommentBodyDto dto)
    {
        var ticketExists = await _context.SupportTickets
            .AnyAsync(t => t.Id == ticketId);

        if (!ticketExists)
            return NotFound($"Ticket {ticketId} not found");

        var comment = new TicketComment
        {
            TicketId = ticketId,
            Comment = dto.Message,
            Type = dto.Type ?? "Internal",
            CommentedByUserId = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketComments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Comment added",
            id = comment.Id,
            comment = comment.Comment,
            createdAt = comment.CreatedAt
        });
    }

    [HttpPost("comment")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentDto dto)
    {
        var comment = new TicketComment
        {
            TicketId = dto.TicketId,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketComments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(comment);
    }

    [HttpGet("{ticketId}/comments")]
    public async Task<IActionResult> GetComments(int ticketId)
    {
        var comments = await _context.TicketComments
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.Comment,
                c.CreatedAt
            })
            .ToListAsync();

        return Ok(comments);
    }


    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteTicket(int id)
    //{
    //    var ticket = await _context.SupportTickets.FindAsync(id);
    //    if (ticket == null)
    //        return NotFound($"Ticket {id} not found");

    //    _context.SupportTickets.Remove(ticket);
    //    await _context.SaveChangesAsync();

    //    return Ok(new { message = "Ticket deleted successfully" });
    //}



    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedBy = User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";
        var result = await _repo.DeleteAsync(id, deletedBy); // or DeleteAsync depending on naming
        if (!result) return NotFound();
        return NoContent();
    }
    
    
    [HttpPut("{id}/softdelete")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var deletedBy = User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";
        var result = await _repo.SoftDeleteAsync(id, deletedBy);

        if (!result) return NotFound();
        return NoContent();
    }
}


