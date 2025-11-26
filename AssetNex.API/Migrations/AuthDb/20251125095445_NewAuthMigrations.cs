using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetNex.API.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class NewAuthMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "463fb724-bf6a-459d-95d2-6e338fe4baf7", "Reader", "READER" },
                    { "570c928b-79ab-4090-bf75-e0cde29a0315", "570c928b-79ab-4090-bf75-e0cde29a0315", "Writer", "WRITER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "f61f8473-db02-4312-b6a5-5871844da9cf", 0, "STATIC-CONCURRENCY-STAMP-12345", "admin@assetnex.com", true, false, null, "ADMIN@ASSETNEX.COM", "ADMIN", "AQAAAAIAAYagAAAAECksSwnnAph3F8RGFvP/wLJx8lQRTdTt0ttF2rWb6lM3MJfZ7X8Zj/olc/Jlz2twPw==", null, false, "STATIC-SECURITY-STAMP-12345", false, "admin" },
                    { "g72g9584-ec13-5423-c7b6-698255eb1eg", 0, "STATIC-CONCURRENCY-STAMP-12345", "user@demo.com", true, false, null, "USER@DEMO.COM", "USER", "N2uIDYJOcFA4bBd2vnAMhM6arpJRBDn6CVxdSTTCwdPGzhSsz6D3ETHPd9BhmFLvYJUWf5qxhyDFcnnrAKd19w==", null, false, "STATIC-SECURITY-STAMP-12345", false, "user" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "f61f8473-db02-4312-b6a5-5871844da9cf" },
                    { "570c928b-79ab-4090-bf75-e0cde29a0315", "f61f8473-db02-4312-b6a5-5871844da9cf" },
                    { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "g72g9584-ec13-5423-c7b6-698255eb1eg" },
                    { "570c928b-79ab-4090-bf75-e0cde29a0315", "g72g9584-ec13-5423-c7b6-698255eb1eg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "f61f8473-db02-4312-b6a5-5871844da9cf" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "570c928b-79ab-4090-bf75-e0cde29a0315", "f61f8473-db02-4312-b6a5-5871844da9cf" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "g72g9584-ec13-5423-c7b6-698255eb1eg" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "570c928b-79ab-4090-bf75-e0cde29a0315", "g72g9584-ec13-5423-c7b6-698255eb1eg" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "463fb724-bf6a-459d-95d2-6e338fe4baf7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "570c928b-79ab-4090-bf75-e0cde29a0315");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f61f8473-db02-4312-b6a5-5871844da9cf");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "g72g9584-ec13-5423-c7b6-698255eb1eg");
        }
    }
}
