using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Dictionaries.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDescriptionLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(30000)",
                maxLength: 30000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10000)",
                oldMaxLength: 10000,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30000)",
                oldMaxLength: 30000,
                oldNullable: true);
        }
    }
}
