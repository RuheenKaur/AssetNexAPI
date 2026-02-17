using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using Asp.Versioning;


namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Services
{
    public interface ISupportTicketService
    {
  //Task AddCommentAsync(int ticketId,AddCommentDto dto, int loggedInUserId);

  //      Task<List<TicketComment>> GetCommentsAsync(int ticketId);

        Task<PagedResult<SupportTicketAdminDto>> GetTicketsAsync(int pageNumber, int pageSize, string? search,
        string? sortField,
        string? sortOrder);
    }
}

