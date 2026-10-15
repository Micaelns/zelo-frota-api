using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class DriverJoinTravelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequeridLicenseCategory",
                table: "VehicleTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Travels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberDriverLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseCategoryDriverLicense = table.Column<int>(type: "int", nullable: false),
                    ExpirationDateDriverLicense = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Travels_DriverId",
                table: "Travels",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Travels_Drivers_DriverId",
                table: "Travels",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Travels_Drivers_DriverId",
                table: "Travels");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Travels_DriverId",
                table: "Travels");

            migrationBuilder.DropColumn(
                name: "RequeridLicenseCategory",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Travels");
        }
    }
}
