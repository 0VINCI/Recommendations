﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Recommendations.Purchase.Core.Data;

#nullable disable

namespace Recommendations.Purchase.Core.Migrations
{
    [DbContext(typeof(PurchaseDbContext))]
    [Migration("20250525194103_OrderPayment")]
    partial class OrderPayment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Purchase")
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Recommendations.Purchase.Core.Data.Models.CustomerDbModel", b =>
                {
                    b.Property<Guid>("IdCustomer")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("IdCustomer");

                    b.ToTable("Customers", "Purchase");
                });

            modelBuilder.Entity("Recommendations.Purchase.Core.Data.Models.OrderDbModel", b =>
                {
                    b.Property<Guid>("IdOrder")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("PaidAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ShippingAddressId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("IdOrder");

                    b.ToTable("Orders", "Purchase");
                });

            modelBuilder.Entity("Recommendations.Purchase.Core.Data.Models.CustomerDbModel", b =>
                {
                    b.OwnsMany("Recommendations.Purchase.Core.Data.Models.AddressDbModel", "Addresses", b1 =>
                        {
                            b1.Property<Guid>("IdAddress")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<Guid>("IdCustomer")
                                .HasColumnType("uuid");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)");

                            b1.HasKey("IdAddress");

                            b1.HasIndex("IdCustomer");

                            b1.ToTable("CustomerAddresses", "Purchase");

                            b1.WithOwner()
                                .HasForeignKey("IdCustomer");
                        });

                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("Recommendations.Purchase.Core.Data.Models.OrderDbModel", b =>
                {
                    b.OwnsMany("Recommendations.Purchase.Core.Data.Models.OrderItemDbModel", "Items", b1 =>
                        {
                            b1.Property<Guid>("IdOrderItem")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("IdOrder")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<string>("ProductName")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)");

                            b1.Property<decimal>("ProductPrice")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.HasKey("IdOrderItem");

                            b1.HasIndex("IdOrder");

                            b1.ToTable("OrderItems", "Purchase");

                            b1.WithOwner()
                                .HasForeignKey("IdOrder");
                        });

                    b.OwnsMany("Recommendations.Purchase.Core.Data.Models.PaymentDbModel", "Payments", b1 =>
                        {
                            b1.Property<Guid>("IdPayment")
                                .HasColumnType("uuid");

                            b1.Property<string>("Details")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)");

                            b1.Property<Guid>("IdOrder")
                                .HasColumnType("uuid");

                            b1.Property<string>("Method")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<DateTime>("PaymentDate")
                                .HasColumnType("timestamp with time zone");

                            b1.HasKey("IdPayment");

                            b1.HasIndex("IdOrder");

                            b1.ToTable("OrderPayments", "Purchase");

                            b1.WithOwner()
                                .HasForeignKey("IdOrder");
                        });

                    b.Navigation("Items");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
