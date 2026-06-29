using Asp.Versioning;
using AssetNex.API.Data;
using AssetNex.API.RepositoriesANI.RepImplementation;
using AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Services;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using System.Text;
using static AssetNex.API.Controllers.AuthController;
using AssetNex.API.Models.DomainModelsANI;


var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration["KeyVaultUrl"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential(),
        new AzureKeyVaultConfigurationOptions());
};

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();   
builder.Services.Configure<JwtSettings>(
builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AssetNexConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbConnection")));

builder.Services.AddScoped<IAssetsAssignmentRep, AssetsAssignmentRep>();
builder.Services.AddScoped<IAssetsRequestsRep, AssetsRequestsRep>();
builder.Services.AddScoped<IAssetsHistoryRep, AssetHistoryRep>();
builder.Services.AddScoped<IAssetsMasterRep, AssetsMasterRep>();
builder.Services.AddScoped<IUsersRep, UsersRep>();
builder.Services.AddScoped<IAssetSoftwareRep, AssetSoftwareRep>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<ISupportTicketsRep, SupportTicketsRep>();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
}).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };

});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        RoleClaimType = "role"
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
    {    
        Console.WriteLine($"JWT FAILED: {context.Exception.Message}");
        return Task.CompletedTask;
},
        OnMessageReceived = context =>
        {
        
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/alerts"))
                context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddAuthorization();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AssetNexIT API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins(
                "http://localhost:4200",
                "https://assetnexangular.z29.web.core.windows.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
        var authDb = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await authDb.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration failed : {ex.Message}");
    }
}  


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<GlobalExceptionMiddleware>();
//app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();
app.MapGet("/test", () => "Working");
app.MapGet("/", () => "RUNNING ");
app.Logger.LogInformation("AssetNexIT API Started");

app.MapGet("/api/minimal/assets", async (IAssetsAssignmentRep repo) =>
{
    var assets = await repo.GetAll();
    return Results.Ok(assets);
});



app.Run();

//app.Use(async (context, next) =>
//{

//    if (context.User.Identity?.IsAuthenticated == true)
//    {
//        var claims = context.User.Claims.Select(c => $"{c.Type} : {c.Value}");
//        Console.WriteLine("JWT Claims:" + string.Join(",", claims));
//    }
//    await next();
//});
public partial class Program { }
  

//public class GlobalExceptionHandler : IExceptionHandler
//{
//    public async ValueTask<bool> TryHandleAsync(HttpContext context, 
//        Exception exception, CancellationToken cancellationToken)
//    {
//        var problemDetails = new ProblemDetails();
//        {
//            Status = StatusCodes.Status500InternalServerError,
//            Title = "An unexpected error occurred",
//            Detail = exception.Message
//        };

//        context.Response.StatusCode = 500;
//        await context.Response.WriteAsJsonAsync(problemDetails);
//        return true;
//    }
//}





