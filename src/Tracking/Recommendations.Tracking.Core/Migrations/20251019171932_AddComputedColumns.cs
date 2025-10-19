using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Tracking.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddComputedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing columns
            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "Tracking",
                table: "events_raw");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "Tracking",
                table: "events_raw");

            // Add computed columns
            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                schema: "Tracking",
                table: "events_raw",
                type: "text",
                nullable: true,
                computedColumnSql: "(\"Payload\"->>'item_id')",
                stored: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                schema: "Tracking",
                table: "events_raw",
                type: "text",
                nullable: true,
                computedColumnSql: "(\"Payload\"->>'order_id')",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop computed columns
            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "Tracking",
                table: "events_raw");

            migrationBuilder.DropColumn(
                name: "OrderId",
                schema: "Tracking",
                table: "events_raw");

            // Add back regular columns
            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                schema: "Tracking",
                table: "events_raw",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                schema: "Tracking",
                table: "events_raw",
                type: "text",
                nullable: true);
        }
    }
}
