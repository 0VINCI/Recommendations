using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Dictionaries.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOnSaleAndProfitBoostColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnSale",
                schema: "Dictionary",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProfitBoost",
                schema: "Dictionary",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsOnSale",
                schema: "Dictionary",
                table: "Products",
                column: "IsOnSale");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProfitBoost",
                schema: "Dictionary",
                table: "Products",
                column: "ProfitBoost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_IsOnSale",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProfitBoost",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsOnSale",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProfitBoost",
                schema: "Dictionary",
                table: "Products");
        }
    }
}
