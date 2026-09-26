using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipients_Name_Id",
                schema: "addresses",
                table: "Recipients");

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                schema: "addresses",
                table: "Locations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "addresses",
                table: "Locations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "addresses",
                table: "Countries",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

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

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                schema: "addresses",
                table: "Locations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "addresses",
                table: "Locations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "addresses",
                table: "Countries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_Name_Id",
                schema: "addresses",
                table: "Recipients",
                columns: new[] { "Name", "Id" },
                unique: true);
        }
    }
}
