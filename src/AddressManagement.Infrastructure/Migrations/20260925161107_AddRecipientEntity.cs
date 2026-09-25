using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AddressManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recipient",
                schema: "addresses",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                schema: "addresses",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Recipients",
                schema: "addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RecipientId",
                schema: "addresses",
                table: "Addresses",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_Name",
                schema: "addresses",
                table: "Recipients",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Recipients_RecipientId",
                schema: "addresses",
                table: "Addresses",
                column: "RecipientId",
                principalSchema: "addresses",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Recipients_RecipientId",
                schema: "addresses",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "Recipients",
                schema: "addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_RecipientId",
                schema: "addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                schema: "addresses",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "Recipient",
                schema: "addresses",
                table: "Addresses",
                type: "text",
                nullable: true);
        }
    }
}
