//using AssetNex.API.Data;
//using AssetNex.API.Hubs;
//using AssetNex.API.Models.DomainModel;
//using Microsoft.AspNetCore.SignalR;

//namespace AssetNex.API.Services
//{
//    public class AlertService : IAlertService
//    {
//        private readonly IHubContext<AlertHub> hub;
//        private readonly ApplicationDbContext dbContext;
//        private readonly ILogger<AlertService> logger;

//        public AlertService(
//            IHubContext<AlertHub> hub,
//            ApplicationDbContext dbContext,
//            ILogger<AlertService> logger)
//        {
//            this.hub = hub;
//            this.dbContext = dbContext;
//            this.logger = logger;

//        }

//        public async Task CheckAndBroadcastLowStockAsync(int assetId, int newStock)
//        {
//            var asset = await dbContext.InventoryAlert.FindAsync(assetId);
//            if (asset == null) return;

//            asset.StockQuantity = newStock;
//            await dbContext.SaveChangesAsync();


//            if (asset.StockQuantity <= asset.Threshold)
//            {
//                var alert = new InventoryAlert
//                {   
//                    AssetId = asset.Id,
//                    AssetName = asset.AssetName,
//                    StockQuantity = asset.StockQuantity,
//                    Threshold = asset.Threshold,
//                    Level = asset.StockQuantity == 0 ? "critical" : "warning",
//                    Message = $"Low stock for {asset.AssetName}: {asset.StockQuantity} left"
//                };

//                await BroadcastAlertAsync(alert);
//            }
//        }
//        public async Task BroadcastAlertAsync(InventoryAlert alert)
//        {
//            await hub.Clients.All.SendAsync("ReceiveInventoryAlert", alert);
//            logger.LogInformation("Broadcasted alert for {Asset} (Id: {Id})", alert.AssetName, alert.AssetId);






//        }


//    }
//}
