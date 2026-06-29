using AssetNex.API.Models.DomainModelsANI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// removed incorrect/useless using directives

namespace AssetNex.API.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<ApplicationUser>();


            var readerRoleId = "463fb724-bf6a-459d-95d2-6e338fe4baf7";
            var writerRoleId = "570c928b-79ab-4090-bf75-e0cde29a0315";
            var adminUserId = "f61f8473-db02-4312-b6a5-5871844da9cf";
           var userId = "b72b9584-ec13-4423-c7b6-698255eb11e9";
            ;


      

            builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@assetnex.com",
                NormalizedUserName = "ADMIN@ASSETNEX.COM",
                Email = "admin@assetnex.com",
                NormalizedEmail = "ADMIN@ASSETNEX.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEJkxsnEzw9Aa+5kAuwYbOQgEc8jgyegwWOBqZFWjr6IHVSig1kTsiArMCA10lI3d1Q==", // This is "Admin@123"
                SecurityStamp = "STATIC-SECURITY-STAMP-12345",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-12345"
            }
        );

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "user",
                    NormalizedUserName = "USER",
                    Email = "user@demo.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "USER@DEMO.COM",
                    PasswordHash = "N2uIDYJOcFA4bBd2vnAMhM6arpJRBDn6CVxdSTTCwdPGzhSsz6D3ETHPd9BhmFLvYJUWf5qxhyDFcnnrAKd19w==",
                    SecurityStamp = "STATIC-SECURITY-STAMP-12345",
                    ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-12345",

                });


          
        }

        public DbSet<RefreshTokenModel> RefreshTokenModel { get; set; }

    }

}


