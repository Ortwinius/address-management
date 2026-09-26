using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestingCOuntryIdForPerformanceImprovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Recipients_Name_Id",
                schema: "addresses",
                table: "Recipients",
                columns: new[] { "Name", "Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipients_Name_Id",
                schema: "addresses",
                table: "Recipients");
        }
    }
}
