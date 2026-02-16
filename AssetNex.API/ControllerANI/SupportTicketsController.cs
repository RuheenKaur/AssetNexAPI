using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/support-tickets")]
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
    public async Task<IActionResult> CreateTicket([FromBody] SupportTicketCreateDto dto)
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
            ResolutionNotes = dto.ResolutionNotes,
            StatusId = 1
        };

        await _repo.CreateAsync(ticket);
        return Ok(ticket);
    }
    
   [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var ticket = await _context.SupportTickets.FindAsync(id);
        if (ticket == null)
            return NotFound($"Ticket {id} not found");

        ticket.StatusId = dto.StatusId;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Status updated successfully", ticket });
    }

    
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateTicketStatusDto dto)
    {
        var ticket = await _context.SupportTickets
            .FirstOrDefaultAsync(x => x.Id == dto.TicketId);

        if (ticket == null)
            return NotFound();

        ticket.StatusId = dto.StatusId;
        await _context.SaveChangesAsync();

        return Ok(ticket);
    }


    [HttpPost("{ticketId}/comment")]
    public async Task<IActionResult> AddCommentToTicket(int ticketId, [FromBody] AddCommentBodyDto dto)
    {
        var ticketExists = await _context.SupportTickets.AnyAsync(t => t.Id == ticketId);
        if (!ticketExists)
            return NotFound($"Ticket {ticketId} not found");

        var comment = new TicketComment
        {
            TicketId = ticketId,
            Comment = dto.Message,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketComments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(comment);
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
        return Ok(await _repo.GetCommentsByTicketIdAsync(ticketId));
    }

 
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.SupportTickets.FindAsync(id);
        if (ticket == null)
            return NotFound($"Ticket {id} not found");
            
        _context.SupportTickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Ticket deleted successfully" });
    }
}

//public class UpdateStatusDto
//{
//    public int StatusId { get; set; }
//}


