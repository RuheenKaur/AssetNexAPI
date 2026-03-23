using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.SupportTicket;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.EntityFrameworkCore;


namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Services
{

    public class SupportTicketService : ISupportTicketService
    {

        private readonly AppDbContext _context;
        private readonly ISupportTicketsRep _repo;

        public SupportTicketService(
            AppDbContext context,
            ISupportTicketsRep repo)

        {
            _context = context;
            _repo = repo;
        }
        public async Task<PagedResult<SupportTicketAdminDto>> GetTicketsAsync(
        int pageNumber,
        int pageSize,
           string? search,
    string? sortField,
    string? sortOrder)
        {
            return await _repo.GetAdminTicketsPagedAsync(
                pageNumber,
                pageSize,
                search,
                sortField,
                sortOrder
            );
        }

    }
}
