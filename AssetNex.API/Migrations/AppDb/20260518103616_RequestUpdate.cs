using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetNex.API.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class RequestUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Users",
                newName: "createdOn");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedAssetType",
                table: "AssetRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "RequestedBy",
                table: "AssetRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WarrantyDate",
                table: "AssetMaster",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "AssetMaster",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AssetMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "AssetHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceTicketId",
                table: "AssetHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "AssetHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CostIncurred",
                table: "AssetHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "AssetHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "AssetHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaster_StatusId",
                table: "AssetMaster",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetAssignments_UserId",
                table: "AssetAssignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetAssignments_Users_UserId",
                table: "AssetAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMaster_StatusMaster_StatusId",
                table: "AssetMaster",
                column: "StatusId",
                principalTable: "StatusMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetAssignments_Users_UserId",
                table: "AssetAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetMaster_StatusMaster_StatusId",
                table: "AssetMaster");

            migrationBuilder.DropIndex(
                name: "IX_AssetMaster_StatusId",
                table: "AssetMaster");

            migrationBuilder.DropIndex(
                name: "IX_AssetAssignments_UserId",
                table: "AssetAssignments");

            migrationBuilder.DropColumn(
                name: "RequestedBy",
                table: "AssetRequests");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AssetMaster");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AssetHistory");

            migrationBuilder.RenameColumn(
                name: "createdOn",
                table: "Users",
                newName: "CreatedOn");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedAssetType",
                table: "AssetRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WarrantyDate",
                table: "AssetMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseDate",
                table: "AssetMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnedDate",
                table: "AssetHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceTicketId",
                table: "AssetHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "AssetHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CostIncurred",
                table: "AssetHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "AssetHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
