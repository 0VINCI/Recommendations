using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace Recommendations.VisualBased.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Visual");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "item_embeddings_visual",
                schema: "Visual",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    Emb = table.Column<Vector>(type: "vector(512)", nullable: false),
                    ModelVer = table.Column<string>(type: "text", nullable: false),
                    GeneratedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_embeddings_visual", x => x.ItemId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_embeddings_visual_GeneratedAt",
                schema: "Visual",
                table: "item_embeddings_visual",
                column: "GeneratedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_embeddings_visual",
                schema: "Visual");
        }
    }
}
