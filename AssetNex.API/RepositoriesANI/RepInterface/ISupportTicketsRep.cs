using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface
{
    public interface ISupportTicketsRep
    {
        Task CreateAsync(SupportTickets ticket);
        Task<List<SupportTickets>> GetUserTicketsAsync(int createdBy);
        Task<List<TicketTrackingDto>> GetUserTicketsWithStatus(int userId);
        Task<PagedResult<SupportTicketAdminDto>> GetAdminTicketsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? sortField,
            string? sortOrder
        );
        Task UpdateStatusAsync(int ticketId, int statusId);
        Task AddCommentAsync(TicketComment comment);
        Task<List<TicketComment>> GetCommentsByTicketIdAsync(int ticketId);
        Task<List<object>> GetAssignedAssetsByUserAsync(int createdBy);
    }
}

