using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AssetNex.API.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class ApplicationUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "b72b9584-ec13-4423-c7b6-698255eb11e9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "570c928b-79ab-4090-bf75-e0cde29a0315", "b72b9584-ec13-4423-c7b6-698255eb11e9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "f61f8473-db02-4312-b6a5-5871844da9cf" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "570c928b-79ab-4090-bf75-e0cde29a0315", "f61f8473-db02-4312-b6a5-5871844da9cf" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "463fb724-bf6a-459d-95d2-6e338fe4baf7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "570c928b-79ab-4090-bf75-e0cde29a0315");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b72b9584-ec13-4423-c7b6-698255eb11e9",
                column: "Contact",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f61f8473-db02-4312-b6a5-5871844da9cf",
                column: "Contact",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "463fb724-bf6a-459d-95d2-6e338fe4baf7", "Reader", "READER" },
                    { "570c928b-79ab-4090-bf75-e0cde29a0315", "570c928b-79ab-4090-bf75-e0cde29a0315", "Writer", "WRITER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "b72b9584-ec13-4423-c7b6-698255eb11e9" },
                    { "570c928b-79ab-4090-bf75-e0cde29a0315", "b72b9584-ec13-4423-c7b6-698255eb11e9" },
                    { "463fb724-bf6a-459d-95d2-6e338fe4baf7", "f61f8473-db02-4312-b6a5-5871844da9cf" },
                    { "570c928b-79ab-4090-bf75-e0cde29a0315", "f61f8473-db02-4312-b6a5-5871844da9cf" }
                });
        }
    }
}
