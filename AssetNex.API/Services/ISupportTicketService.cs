using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using Asp.Versioning;


namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Services
{
    public interface ISupportTicketService
    {
        Task<PagedResult<SupportTicketAdminDto>> GetTicketsAsync(int pageNumber, int pageSize, string? search,
        string? sortField,
        string? sortOrder);
    }
}

