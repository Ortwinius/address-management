using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedLocationIndexesToBeNonUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name",
                schema: "addresses",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ZipCode",
                schema: "addresses",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                schema: "addresses",
                table: "Locations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ZipCode",
                schema: "addresses",
                table: "Locations",
                column: "ZipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name",
                schema: "addresses",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ZipCode",
                schema: "addresses",
                table: "Locations");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                schema: "addresses",
                table: "Locations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ZipCode",
                schema: "addresses",
                table: "Locations",
                column: "ZipCode",
                unique: true);
        }
    }
}
