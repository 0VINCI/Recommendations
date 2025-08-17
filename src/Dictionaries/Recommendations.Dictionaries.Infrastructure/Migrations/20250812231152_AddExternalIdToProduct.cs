using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Dictionaries.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalIdToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                schema: "Dictionary",
                table: "Products",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "Dictionary",
                table: "Products");
        }
    }
}
