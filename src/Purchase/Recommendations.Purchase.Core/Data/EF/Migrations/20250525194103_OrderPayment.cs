using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recommendations.Purchase.Core.Migrations
{
    /// <inheritdoc />
    public partial class OrderPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE SCHEMA IF NOT EXISTS ""Purchase"";");

            migrationBuilder.DropTable(
                name: "CustomerPayments",
                schema: "Purchase");

            migrationBuilder.CreateTable(
                name: "OrderPayments",
                schema: "Purchase",
                columns: table => new
                {
                    IdPayment = table.Column<Guid>(type: "uuid", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Details = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdOrder = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPayments", x => x.IdPayment);
                    table.ForeignKey(
                        name: "FK_OrderPayments_Orders_IdOrder",
                        column: x => x.IdOrder,
                        principalSchema: "Purchase",
                        principalTable: "Orders",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_IdOrder",
                schema: "Purchase",
                table: "OrderPayments",
                column: "IdOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPayments",
                schema: "Purchase");

            migrationBuilder.CreateTable(
                name: "CustomerPayments",
                schema: "Purchase",
                columns: table => new
                {
                    IdPayment = table.Column<Guid>(type: "uuid", nullable: false),
                    Details = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdCustomer = table.Column<Guid>(type: "uuid", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPayments", x => x.IdPayment);
                    table.ForeignKey(
                        name: "FK_CustomerPayments_Customers_IdCustomer",
                        column: x => x.IdCustomer,
                        principalSchema: "Purchase",
                        principalTable: "Customers",
                        principalColumn: "IdCustomer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_IdCustomer",
                schema: "Purchase",
                table: "CustomerPayments",
                column: "IdCustomer");
        }
    }
}
