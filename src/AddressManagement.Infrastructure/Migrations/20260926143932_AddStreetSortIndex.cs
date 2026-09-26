using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStreetSortIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Street_Id",
                schema: "addresses",
                table: "Addresses",
                columns: new[] { "Street", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_Street_Id",
                schema: "addresses",
                table: "Addresses");
        }
    }
}
