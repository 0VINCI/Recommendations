using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Dictionaries.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dictionary");

            migrationBuilder.CreateTable(
                name: "BaseColours",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseColours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterCategories",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MasterCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_MasterCategories_MasterCategoryId",
                        column: x => x.MasterCategoryId,
                        principalSchema: "Dictionary",
                        principalTable: "MasterCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTypes",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SubCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTypes_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalSchema: "Dictionary",
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductDisplayName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BrandName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Rating = table.Column<decimal>(type: "numeric(3,1)", nullable: false),
                    Reviews = table.Column<int>(type: "integer", nullable: false),
                    IsBestseller = table.Column<bool>(type: "boolean", nullable: false),
                    IsNew = table.Column<bool>(type: "boolean", nullable: false),
                    SubCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseColourId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalSchema: "Dictionary",
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_BaseColours_BaseColourId",
                        column: x => x.BaseColourId,
                        principalSchema: "Dictionary",
                        principalTable: "BaseColours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalSchema: "Dictionary",
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Season = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Usage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Year = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SleeveLength = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fabric = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Collar = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BodyOrGarmentSize = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pattern = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AgeGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Dictionary",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                schema: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ImageType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Resolution = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Dictionary",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_Name",
                schema: "Dictionary",
                table: "ArticleTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypes_SubCategoryId",
                schema: "Dictionary",
                table: "ArticleTypes",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseColours_Name",
                schema: "Dictionary",
                table: "BaseColours",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterCategories_Name",
                schema: "Dictionary",
                table: "MasterCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Gender",
                schema: "Dictionary",
                table: "ProductDetails",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ProductId",
                schema: "Dictionary",
                table: "ProductDetails",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Season",
                schema: "Dictionary",
                table: "ProductDetails",
                column: "Season");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_Usage",
                schema: "Dictionary",
                table: "ProductDetails",
                column: "Usage");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ImageType",
                schema: "Dictionary",
                table: "ProductImages",
                column: "ImageType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_IsPrimary",
                schema: "Dictionary",
                table: "ProductImages",
                column: "IsPrimary");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                schema: "Dictionary",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ArticleTypeId",
                schema: "Dictionary",
                table: "Products",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BaseColourId",
                schema: "Dictionary",
                table: "Products",
                column: "BaseColourId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandName",
                schema: "Dictionary",
                table: "Products",
                column: "BrandName");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsBestseller",
                schema: "Dictionary",
                table: "Products",
                column: "IsBestseller");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsNew",
                schema: "Dictionary",
                table: "Products",
                column: "IsNew");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                schema: "Dictionary",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Rating",
                schema: "Dictionary",
                table: "Products",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                schema: "Dictionary",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_MasterCategoryId",
                schema: "Dictionary",
                table: "SubCategories",
                column: "MasterCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_Name",
                schema: "Dictionary",
                table: "SubCategories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDetails",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "ProductImages",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "ArticleTypes",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "BaseColours",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "SubCategories",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "MasterCategories",
                schema: "Dictionary");
        }
    }
}
