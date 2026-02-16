using AssetNex.API.Models.DomainModel;

namespace AssetNex.API.Services
{
    public interface IAlertService
    {
        Task CheckAndBroadcastLowStockAsync(int assetId, int newStock);
        Task BroadcastAlertAsync(InventoryAlert alert);

    }
}


//using Microsoft.Extensions.Caching.Memory;

//public class AssetService : IAssetService
//{
//    private readonly IAssetRepository _repo;
//    private readonly IMemoryCache _cache;
//    private const string ASSETS_CACHE_KEY = "all_assets";
//    private const int CACHE_DURATION_MINUTES = 30;

//    public AssetService(IAssetRepository repo, IMemoryCache cache)
//    {
//        _repo = repo;
//        _cache = cache;
//    }

//    // WITHOUT CACHING (OLD):
//    // public async Task<List<Asset>> GetAllAssetsAsync()
//    // {
//    //     return await _repo.GetAllAsync();
//    // }

//    // WITH CACHING (NEW):
//    public async Task<List<Asset>> GetAllAssetsAsync()
//    {
//        // Try to get from cache first
//        if (_cache.TryGetValue(ASSETS_CACHE_KEY, out List<Asset> cachedAssets))
//        {
//            Console.WriteLine("✅ Returning assets from CACHE");
//            return cachedAssets;
//        }

//        // Not in cache, fetch from database
//        Console.WriteLine("⚠️ Cache MISS - Fetching from DATABASE");
//        var assets = await _repo.GetAllAsync();

//        // Store in cache for 30 minutes
//        var cacheOptions = new MemoryCacheEntryOptions
//        {
//            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_DURATION_MINUTES)
//        };

//        _cache.Set(ASSETS_CACHE_KEY, assets, cacheOptions);

//        return assets;
//    }

//    // Cache Invalidation - Call this when asset is created/updated/deleted
//    public async Task<Asset> CreateAssetAsync(Asset asset)
//    {
//        var created = await _repo.AddAsync(asset);

//        // Invalidate cache so next request gets fresh data
//        _cache.Remove(ASSETS_CACHE_KEY);
//        Console.WriteLine("🔄 Cache INVALIDATED after creating asset");

//        return created;
//    }

//    public async Task<Asset> UpdateAssetAsync(Asset asset)
//    {
//        var updated = await _repo.UpdateAsync(asset);

//        // Invalidate cache
//        _cache.Remove(ASSETS_CACHE_KEY);
//        Console.WriteLine("🔄 Cache INVALIDATED after updating asset");

//        return updated;
//    }

//    public async Task<bool> DeleteAssetAsync(int id)
//    {
//        var deleted = await _repo.DeleteAsync(id);

//        // Invalidate cache
//        _cache.Remove(ASSETS_CACHE_KEY);
//        Console.WriteLine("🔄 Cache INVALIDATED after deleting asset");

//        return deleted;
//    }
//}


//public async Task<User> GetUserByIdAsync(int userId)
//{
//    string cacheKey = $"user_{userId}";  // Unique key per user

//    // Check cache
//    if (_cache.TryGetValue(cacheKey, out User cachedUser))
//    {
//        return cachedUser;
//    }

//    // Fetch from DB
//    var user = await _repo.GetByIdAsync(userId);

//    // Cache for 15 minutes
//    _cache.Set(cacheKey, user, TimeSpan.FromMinutes(15));

//    return user;
//}

//// Invalidate when user updates profile
//public async Task<User> UpdateUserProfileAsync(int userId, UpdateUserDto dto)
//{
//    var user = await _repo.UpdateAsync(userId, dto);

//    // Remove from cache
//    _cache.Remove($"user_{userId}");

//    return user;
//}



//public class LoggingMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<LoggingMiddleware> _logger;

//    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        // Log request
//        _logger.LogInformation($"➡️ Request: {context.Request.Method} {context.Request.Path}");

//        // Call next middleware
//        await _next(context);

//        // Log response
//        _logger.LogInformation($"⬅️ Response: {context.Response.StatusCode}");
//    }
//}



//public class GlobalExceptionMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<GlobalExceptionMiddleware> _logger;

//    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"❌ Error: {ex.Message}");

//            context.Response.StatusCode = 500;
//            context.Response.ContentType = "application/json";

//            await context.Response.WriteAsJsonAsync(new
//            {
//                error = "Internal Server Error",
//                message = ex.Message
//            });
//        }
//    }
//}


