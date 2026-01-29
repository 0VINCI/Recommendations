using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace Recommendations.Tracking.Core.Migrations.SignalsDb
{
    /// <inheritdoc />
    public partial class AddItemEmbeddingsVisual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "item_embeddings_visual",
                schema: "Tracking",
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
                name: "ix_item_embeddings_visual_generated_at",
                schema: "Tracking",
                table: "item_embeddings_visual",
                column: "GeneratedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_item_embeddings_visual_generated_at",
                schema: "Tracking",
                table: "item_embeddings_visual");

            migrationBuilder.DropTable(
                name: "item_embeddings_visual",
                schema: "Tracking");
        }
    }
}

