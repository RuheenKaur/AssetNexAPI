

using System.Text;
using AssetNex.API.Controllers;
using AssetNex.API.Data;
using AssetNex.API.Hubs;
using AssetNex.API.Repositories.Implementation;
using AssetNex.API.Repositories.Interface;
using AssetNex.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();//logging basic 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


//builder.Services.AddSerilog(Options =>
//{
//    Options.ReadFrom.Configuration
//    (builder.Configuration);
//});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();

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
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<ISoftwareLicenseRepository, SoftwareLicenseRepository>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.Configure<AuthController.JwtSettings>
    (builder.Configuration.GetSection("JwtSettings"));

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
                 Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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
Console.WriteLine("JWT KEY -> " + builder.Configuration["JwtSettings:Key"]);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertHub>("/hubs/alerts");
app.Logger.LogInformation("Program Started");
app.Run();


//// Identity
//builder.Services
//    .AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<AuthDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequiredUniqueChars = 0;
//});

//// Register Repositories
//builder.Services.AddScoped<IEWasteRepository, EWasteRepository>();
//builder.Services.AddScoped<IAssetsRepository, AssetsRepository>();
//builder.Services.AddScoped<ISupportRepository, SupportRepository>();
//builder.Services.AddScoped<IHardwareRepository, HardwareRepository>();
//builder.Services.AddScoped<ILocationRepository, LocationRepository>();
//builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();
//builder.Services.AddScoped<IAlertService, AlertService>();
//builder.Services.AddScoped<ISoftwareLicenseRepository, SoftwareLicenseRepository>();

//// Bind JwtSettings section (correct)
//builder.Services.Configure<AuthController.JwtSettings>(
//    builder.Configuration.GetSection("JwtSettings")
//);

//// Authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<AuthController.JwtSettings>();

//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings.Issuer,
//            ValidAudience = jwtSettings.Audience,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
//            ClockSkew = TimeSpan.Zero
//        };

//        options.Events = new JwtBearerEvents
//        {
//            OnMessageReceived = context =>
//            {
//                var accessToken = context.Request.Query["access_token"];
//                var path = context.HttpContext.Request.Path;

//                if (!string.IsNullOrEmpty(accessToken) &&
//                    path.StartsWithSegments("/hubs/alerts"))
//                {
//                    context.Token = accessToken;
//                }

//                return Task.CompletedTask;
//            }
//        };
//    });

//// Swagger Auth
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AssetNexIT API", Version = "v1" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Insert JWT token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

//// Authorization
//builder.Services.AddAuthorization();

//// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular",
//        policy => policy
//            .WithOrigins("https://localhost:4200")
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials());
//});

//var app = builder.Build();

//app.UseCors("AllowAngular");

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Do NOT log the key for security.
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//app.MapHub<AlertHub>("/hubs/alerts");
//app.Logger.LogInformation("Program Started");
//app.Run();
