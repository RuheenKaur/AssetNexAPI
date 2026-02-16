using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetNex.API.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f61f8473-db02-4312-b6a5-5871844da9cf",
                column: "UserName",
                value: "admin@assetnex.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f61f8473-db02-4312-b6a5-5871844da9cf",
                column: "UserName",
                value: "admin@assenex.com");
        }
    }
}
