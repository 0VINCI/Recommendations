using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace Recommendations.ContentBased.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddProductEmbeddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS vector;");
            
            migrationBuilder.EnsureSchema(
                name: "Vectors");

            migrationBuilder.CreateTable(
                name: "ProductEmbeddings",
                schema: "Vectors",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Variant = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Embedding = table.Column<Vector>(type: "vector(768)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEmbeddings", x => new { x.ProductId, x.Variant });
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddings_CreatedAt",
                schema: "Vectors",
                table: "ProductEmbeddings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddings_ProductId",
                schema: "Vectors",
                table: "ProductEmbeddings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEmbeddings_Variant",
                schema: "Vectors",
                table: "ProductEmbeddings",
                column: "Variant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductEmbeddings",
                schema: "Vectors");
        }
    }
}
