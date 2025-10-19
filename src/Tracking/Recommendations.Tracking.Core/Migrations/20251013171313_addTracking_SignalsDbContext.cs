using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace Recommendations.Tracking.Core.Migrations.SignalsDb
{
    /// <inheritdoc />
    public partial class addTracking_SignalsDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tracking");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "item_embeddings_cf",
                schema: "Tracking",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    Emb = table.Column<Vector>(type: "vector(768)", nullable: false),
                    ModelVer = table.Column<string>(type: "text", nullable: false),
                    TrainedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_embeddings_cf", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "user_embeddings_cf",
                schema: "Tracking",
                columns: table => new
                {
                    UserKey = table.Column<string>(type: "text", nullable: false),
                    Emb = table.Column<Vector>(type: "vector(768)", nullable: false),
                    ModelVer = table.Column<string>(type: "text", nullable: false),
                    TrainedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_embeddings_cf", x => x.UserKey);
                });

            migrationBuilder.CreateTable(
                name: "user_embeddings_online",
                schema: "Tracking",
                columns: table => new
                {
                    UserKey = table.Column<string>(type: "text", nullable: false),
                    Emb = table.Column<Vector>(type: "vector(768)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_embeddings_online", x => x.UserKey);
                });

            migrationBuilder.CreateTable(
                name: "user_item_interactions",
                schema: "Tracking",
                columns: table => new
                {
                    UserKey = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    Views = table.Column<int>(type: "integer", nullable: false),
                    Clicks = table.Column<int>(type: "integer", nullable: false),
                    Carts = table.Column<int>(type: "integer", nullable: false),
                    Purchases = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    LastTs = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_item_interactions", x => new { x.UserKey, x.ItemId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_embeddings_cf_TrainedAt",
                schema: "Tracking",
                table: "item_embeddings_cf",
                column: "TrainedAt");

            migrationBuilder.CreateIndex(
                name: "IX_user_embeddings_cf_TrainedAt",
                schema: "Tracking",
                table: "user_embeddings_cf",
                column: "TrainedAt");

            migrationBuilder.CreateIndex(
                name: "IX_user_embeddings_online_UpdatedAt",
                schema: "Tracking",
                table: "user_embeddings_online",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "ix_uii_last_ts",
                schema: "Tracking",
                table: "user_item_interactions",
                column: "LastTs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_embeddings_cf",
                schema: "Tracking");

            migrationBuilder.DropTable(
                name: "user_embeddings_cf",
                schema: "Tracking");

            migrationBuilder.DropTable(
                name: "user_embeddings_online",
                schema: "Tracking");

            migrationBuilder.DropTable(
                name: "user_item_interactions",
                schema: "Tracking");
        }
    }
}
