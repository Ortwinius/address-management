using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTrigramDirectiveForStreet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_Street",
                schema: "addresses",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Street",
                schema: "addresses",
                table: "Addresses",
                column: "Street")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_Street",
                schema: "addresses",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Street",
                schema: "addresses",
                table: "Addresses",
                column: "Street");
        }
    }
}
