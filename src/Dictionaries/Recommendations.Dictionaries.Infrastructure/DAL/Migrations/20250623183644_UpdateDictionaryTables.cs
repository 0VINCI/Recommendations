using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Dictionaries.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDictionaryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_BaseColours_BaseColourId1",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BaseColourId1",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BaseColourId1",
                schema: "Dictionary",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Usage",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Fit",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FashionType",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30000)",
                oldMaxLength: 30000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Colour2",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Colour1",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArticleNumber",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AgeGroup",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BaseColourId1",
                schema: "Dictionary",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Usage",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Fit",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FashionType",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(30000)",
                maxLength: 30000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Colour2",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Colour1",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArticleNumber",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AgeGroup",
                schema: "Dictionary",
                table: "ProductDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BaseColourId1",
                schema: "Dictionary",
                table: "Products",
                column: "BaseColourId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BaseColours_BaseColourId1",
                schema: "Dictionary",
                table: "Products",
                column: "BaseColourId1",
                principalSchema: "Dictionary",
                principalTable: "BaseColours",
                principalColumn: "Id");
        }
    }
}
