using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetNex.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckedOutBy",
                table: "AssetLocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CheckedOutByEmail",
                table: "AssetLocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateTime",
                table: "AssetLocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InventoryAlert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAlert", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryAlert");

            migrationBuilder.DropColumn(
                name: "CheckedOutBy",
                table: "AssetLocations");

            migrationBuilder.DropColumn(
                name: "CheckedOutByEmail",
                table: "AssetLocations");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "AssetLocations");
        }
    }
}
