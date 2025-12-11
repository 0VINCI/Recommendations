using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace Recommendations.ContentBased.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddProductEmbeddingsNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "ProductEmbeddingsNew",
                schema: "Vectors",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Variant = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Embedding = table.Column<Vector>(type: "vector(2560)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEmbeddingsNew", x => new { x.ProductId, x.Variant });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddingsNew_CreatedAt",
                schema: "Vectors",
                table: "ProductEmbeddingsNew",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddingsNew_ProductId",
                schema: "Vectors",
                table: "ProductEmbeddingsNew",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddingsNew_Variant",
                schema: "Vectors",
                table: "ProductEmbeddingsNew",
                column: "Variant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductEmbeddingsNew",
                schema: "Vectors");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:vector", ",,");
        }
    }
}
