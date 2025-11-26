using AssetNex.API.Models.DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetNex.API.Data
{
    public class AuthDbContext : IdentityDbContext<IdentityUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "463fb724-bf6a-459d-95d2-6e338fe4baf7";
            var writerRoleId = "570c928b-79ab-4090-bf75-e0cde29a0315";
            var adminUserId = "f61f8473-db02-4312-b6a5-5871844da9cf";
            var userId = "g72g9584-ec13-5423-c7b6-698255eb1eg";


            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER",
                    ConcurrencyStamp = readerRoleId
                },

                new IdentityRole
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER",
                    ConcurrencyStamp = writerRoleId
                }
                );

            builder.Entity<IdentityUser>().HasData(
            new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@assetnex.com",
                NormalizedEmail = "ADMIN@ASSETNEX.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAECksSwnnAph3F8RGFvP/wLJx8lQRTdTt0ttF2rWb6lM3MJfZ7X8Zj/olc/Jlz2twPw==", // This is "Admin@123"
                SecurityStamp = "STATIC-SECURITY-STAMP-12345",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-12345"
            }
        );

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = userId,
                    UserName = "user",
                    NormalizedUserName = "USER",
                    Email = "user@demo.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "USER@DEMO.COM",
                    PasswordHash = "N2uIDYJOcFA4bBd2vnAMhM6arpJRBDn6CVxdSTTCwdPGzhSsz6D3ETHPd9BhmFLvYJUWf5qxhyDFcnnrAKd19w==",
                    SecurityStamp = "STATIC-SECURITY-STAMP-12345",
                    ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-12345"
                });


            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = readerRoleId,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = writerRoleId,
                    UserId = adminUserId
                });

            builder.Entity<IdentityUserRole<string>>().HasData
         (
             new IdentityUserRole<string>
             {
                 RoleId = readerRoleId,
                 UserId = userId
             },

             new IdentityUserRole<string>
             {
                 RoleId = writerRoleId,
                 UserId = userId
             });


        }

        public DbSet<RefreshTokenModel> RefreshTokenModel { get; set; }

    }

}


//using AssetNex.API.Models.DomainModel;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace AssetNex.API.Data
//{
//    public class AuthDbContext : IdentityDbContext<IdentityUser>
//    {
//        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            // 🚨 No seeding here — removes all duplication issues
//        }

//        public DbSet<RefreshTokenModel> RefreshTokenModel { get; set; }
//    }
//}
