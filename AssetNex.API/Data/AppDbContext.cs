using Microsoft.EntityFrameworkCore;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using AssetNex.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AssetsMaster> AssetMaster { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<AssetRequests> AssetRequests { get; set; }
    public DbSet<Asset_Software> Asset_Software { get; set; }
    public DbSet<SupportTickets> SupportTickets { get; set; }
    public DbSet<AssetAssignments> AssetAssignments { get; set; }
    public DbSet<AssetsHistory> AssetHistory { get; set; }
    public DbSet<StatusMaster> StatusMaster { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SupportTickets>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy);
    }
}