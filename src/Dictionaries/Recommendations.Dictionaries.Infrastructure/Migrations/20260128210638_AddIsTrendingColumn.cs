using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Dictionaries.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTrendingColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTrending",
                schema: "Dictionary",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsTrending",
                schema: "Dictionary",
                table: "Products",
                column: "IsTrending");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_IsTrending",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsTrending",
                schema: "Dictionary",
                table: "Products");
        }
    }
}
