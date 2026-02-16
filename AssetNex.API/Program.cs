
using AssetNex.API.Data;
using AssetNex.API.Hubs;
using AssetNex.API.Models.DomainModel;
using AssetNex.API.Repositories.Implementation;
using AssetNex.API.Repositories.Interface;
using AssetNex.API.RepositoriesANI.RepImplementation;
using AssetNex.API.RepositoriesANI.RepInterface;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepImplementation;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.RepositoriesANI.RepInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Services;
using System.Text;
using static AssetNex.API.Controllers.AuthController;
using static Dropbox.Api.TeamLog.ActorLogInfo;
using Microsoft.IdentityModel.Tokens.Experimental;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Logging.AddConsole();
builder.Services.AddRazorComponents();
builder.Logging.ClearProviders();
builder.Services.AddServerSideBlazor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AssetNexConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AssetNexConnectionString")));

    builder.Services.AddDbContext<AuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbConnection")));



    builder.Services.AddScoped<IEWasteRepository, EWasteRepository>();
    builder.Services.AddScoped<IAssetsRepository, AssetsRepository>();
    builder.Services.AddScoped<ISupportRepository, SupportRepository>();
    builder.Services.AddScoped<IHardwareRepository, HardwareRepository>();
    builder.Services.AddScoped<ILocationRepository, LocationRepository>();
    builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();  
    builder.Services.AddScoped<ISoftwareLicenseRepository, SoftwareLicenseRepository>();
    

builder.Services.AddScoped<IAssetsAssignmentRep, AssetsAssignmentRep>();
builder.Services.AddScoped<IAssetsRequestsRep, AssetsRequestsRep>();
builder.Services.AddScoped<IAssetsHistoryRep, AssetHistoryRep>();
builder.Services.AddScoped<IAssetsMasterRep, AssetsMasterRep>();
builder.Services.AddScoped<IUsersRep,UsersRep>();
builder.Services.AddScoped<IAssetSoftwareRep,AssetSoftwareRep>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<ISupportTicketsRep, SupportTicketsRep>();

builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("hubs/alerts"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

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

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});// addapiversioning is an extension method, takes an ApiVersioningObjects 

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
                    Id="Bearer"
                }
            },
            new string[] {}
        }
    });

});

//app.UseMiddleware<GlobalExceptionMiddleware>(); 

builder.Services
.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular",
            policy => policy.WithOrigins("https://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod());
    });


var app = builder.Build();



app.UseCors(builder =>
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

    }

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services);
}
Console.WriteLine("JWT KEY -> " + builder.Configuration["JwtSettings:Key"]);

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapRazorComponents<App>(); 
    app.MapHub<AlertHub>("/hubs/alerts");
    app.UseStaticFiles();
    app.Logger.LogInformation("Program Started");
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.Run();


// This reads from Railway/Render environment variables
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? Environment.GetEnvironmentVariable("DATABASE_URL");

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(connectionString));  // or UseSqlServer for SQL Server

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowVercelFrontend", policy =>
//    {
//        policy.WithOrigins(
//            "https://your-app.vercel.app", 
//            "http://localhost:4200"       
//        )
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials();
//    });
//});

//// AFTER var app = builder.Build();

//app.UseCors("AllowVercelFrontend");  
//app.UseAuthorization();


//{
//    "ConnectionStrings": {
//        "DefaultConnection": "Server=your-railway-db-url;Database=AssetNexIT;User Id=postgres;Password=xxx;"
//    },
//  "Logging": {
//        "LogLevel": {
//            "Default": "Information"
//        }
//    },
//  "AllowedHosts": "*"
//}

//private readonly IMemoryCache _cache;

//public async Task<List<Status>> GetStatusesAsync()
//{
//if (!_cache.TryGetValue("statuses", out List<Status> statuses))
//{
//statuses = await _repo.GetAllStatusesAsync();
//_cache.Set("statuses", statuses, TimeSpan.FromHours(1));
//}
//return statuses;
//}



//public class TicketRepositoryTests
//{
//    [Fact]
//    public void Skip_Take_Calculation_Is_Correct()
//    {
//        // Arrange
//        int pageNumber = 2;
//        int pageSize = 10;

//        // Act
//        int skip = (pageNumber - 1) * pageSize;

//        // Assert
//        Assert.Equal(10, skip); // Page 2 should skip 10 records
//    }
//}
//D) Integration Testing - Do you have ANY? (Probably NO)
//Be honest:

//"I've been testing manually through Swagger and the UI." +
//    " I should add automated integration tests that call API endpoints and verify responses." +
//    " This would catch bugs before manual testing."

//express ruby on rails , springboot