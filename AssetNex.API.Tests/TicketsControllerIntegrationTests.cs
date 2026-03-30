using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;
using AssetNex.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TicketsControllerIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;


    public TicketsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {

        _client = factory.CreateClient();

        _client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", GenerateTestToken());
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                var authDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AuthDbContext>));
                if (authDescriptor != null) services.Remove(authDescriptor);

              
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("TestAppDb"));

                services.AddDbContext<AuthDbContext>(options =>
                    options.UseInMemoryDatabase("TestAuthDb"));
            });
        }).CreateClient();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateTestToken());
    }
   
    private string GenerateTestToken()
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_123456"));
        var claims = new[]
        {
            new Claim("id", "1"),
            new Claim(ClaimTypes.Name, "TestAdmin"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var token = new JwtSecurityToken(
            issuer: "AssetNexAPI",
            audience: "AssetNexClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [Fact]
    public async Task GetAdminTickets_ReturnsOk()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateTestToken());
        var response = await _client.GetAsync("/api/support-tickets/admin");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task GetAdminTickets_ReturnsData()
    {
        var response = await _client.GetAsync("/api/support-tickets/admin");
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content);
        Assert.True(content.Length > 0);
    }
    [Fact]
    public async Task GetTickets_WithToken_Returns200()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateTestToken());
        var response = await _client.GetAsync("/api/support-tickets");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task GetTicketsByUser_ReturnsOk()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateTestToken());
        var response = await _client.GetAsync("/api/support-tickets/user/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public void Dummy_Test()
    {
        Assert.True(true);
    }
    [Fact]
    public async Task GetAdminTickets_WithoutToken_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", GenerateTestToken());
        var factory = new WebApplicationFactory<Program>();
        var unauthClient = factory.CreateClient();
        var response = await unauthClient.GetAsync("/api/support-tickets/admin");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
