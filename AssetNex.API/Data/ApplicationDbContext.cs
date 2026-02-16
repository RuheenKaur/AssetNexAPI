using AssetNex.API.Models.DomainModel;
using AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DomainModelsANI;
using Microsoft.EntityFrameworkCore;

namespace AssetNex.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SupportTickets>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<AssetInfo> AssetInfo { get; set; }
        public DbSet<Assign> Assign { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<EWaste> EWastes { get; set; }  
        public DbSet<EWaste> DisposedAssets { get; set; }
        public DbSet<EWaste> DisposableAssets { get; set; }
        public DbSet<SoftwareLicenseInfo> SoftwareLicenseInfo { get; set; }
        public DbSet<Hardware> Hardware { get; set; }
        public DbSet<NewSupport> NewSupports { get; set; }
        public DbSet<LiveLocation> Location { get; set; }
        public DbSet<AssetLocations> AssetLocations { get; set; }
        public DbSet<InventoryAlert> InventoryAlert { get; set; }

    }
}







