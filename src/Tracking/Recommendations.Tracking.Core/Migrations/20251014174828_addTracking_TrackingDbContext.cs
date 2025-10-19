using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Tracking.Core.Migrations
{
    /// <inheritdoc />
    public partial class addTracking_TrackingDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tracking");

            migrationBuilder.EnsureSchema(
                name: "rec");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:rec.event_source", "frontend,backend");

            migrationBuilder.CreateTable(
                name: "events_raw",
                schema: "Tracking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ts = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<int>(type: "rec.event_source", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    AnonymousId = table.Column<Guid>(type: "uuid", nullable: true),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Context = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    Payload = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events_raw", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "events_rejected",
                schema: "Tracking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    Raw = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events_rejected", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "identity_links",
                schema: "Tracking",
                columns: table => new
                {
                    AnonymousId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LinkedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_links", x => new { x.AnonymousId, x.UserId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_events_raw_AnonymousId",
                schema: "Tracking",
                table: "events_raw",
                column: "AnonymousId");

            migrationBuilder.CreateIndex(
                name: "IX_events_raw_ItemId",
                schema: "Tracking",
                table: "events_raw",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_events_raw_OrderId",
                schema: "Tracking",
                table: "events_raw",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_events_raw_Type_Ts",
                schema: "Tracking",
                table: "events_raw",
                columns: new[] { "Type", "Ts" });

            migrationBuilder.CreateIndex(
                name: "IX_events_raw_UserId",
                schema: "Tracking",
                table: "events_raw",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events_raw",
                schema: "Tracking");

            migrationBuilder.DropTable(
                name: "events_rejected",
                schema: "Tracking");

            migrationBuilder.DropTable(
                name: "identity_links",
                schema: "Tracking");
        }
    }
}
