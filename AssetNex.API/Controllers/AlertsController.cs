using AssetNex.API.Hubs;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Repositories.Interface;
using AssetNex.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AssetNex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertsRepository alertsRepository;
        private readonly IAlertService alertService;
        private readonly IHubContext<AlertHub> _hubContext;
        private readonly IClientProxy clientProxy;
        public AlertsController(IAlertsRepository alertsRepository, IAlertService alertService, IHubContext hubContext)
        {
            this.alertsRepository = alertsRepository;
            this.alertService = alertService;
            //_hubContext = (IHubContext<AlertHub>?)hubContext;
        }

        [HttpPost("updatestock/{assetId}")]
        public async Task<IActionResult> UpdateStock(int assetId, [FromBody] int newStock)
        {

            await alertService.CheckAndBroadcastLowStockAsync(assetId, newStock);
            //await _hubContext.Clients.All.SendAsync("Low Quantity");
            return Ok(new { Message = "Stock updated and alert check triggered successfully" });
        }


        [HttpGet("realalerts")]
        public async Task<IActionResult> GetRealAlerts()
        {
            var assets = await alertsRepository.getAllStock();

            var response = assets.Select(stock => new InventoryAlert
            {
                Id = stock.Id,
                AssetId = stock.AssetId,
                AssetName = stock.AssetName,
                Threshold = stock.Threshold,
                StockQuantity = stock.StockQuantity,
                Level = stock.Level,
                Message = stock.Message,
                CreatedAt = stock.CreatedAt,


            }).ToList();

            return Ok(response);
        }


        [HttpDelete("deleterealalerts")]
        public async Task<IActionResult> DeleteRealAlerts([FromBody] int id)
        {
            var currentRealAlert = await alertsRepository.GetByIdAsync(id);

            if (currentRealAlert == null)
                return NotFound(new { Message = "Alert not found" });

            await alertsRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
