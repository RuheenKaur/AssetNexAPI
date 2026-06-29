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
    public DbSet<Department> Department { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SupportTickets>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy);


        modelBuilder.Entity<AssetsHistory>()
          .HasOne(h => h.Status)
          .WithMany()
          .HasForeignKey(h => h.StatusId)
          .OnDelete(DeleteBehavior.Restrict);



        modelBuilder.Entity<AssetRequests>()
        .HasOne(r => r.Status)
        .WithMany()
        .HasForeignKey(r => r.StatusId)
        .OnDelete(DeleteBehavior.Restrict);



        modelBuilder.Entity<SupportTickets>()
        .HasOne(s => s.Status) 
        .WithMany()
        .HasForeignKey(s => s.StatusId)
        .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<AssetsMaster>().HasQueryFilter(a => !a.IsDeleted);

        modelBuilder.Entity<SupportTickets>().HasQueryFilter(t => !t.IsDeleted);

        modelBuilder.Entity<AssetRequests>().HasQueryFilter(r => !r.IsDeleted);

    }
}
