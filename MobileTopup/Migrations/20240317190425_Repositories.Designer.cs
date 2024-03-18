﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobileTopup.Infrastructure;

#nullable disable

namespace MobileTopup.API.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240317190425_Repositories")]
    partial class Repositories
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Beneficiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Beneficiaries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            NickName = "John Doe",
                            PhoneNumber = "1234567890",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.TopupHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TopupHistories");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.TopupOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("TopupOptions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 5m,
                            Name = "AED5"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 10m,
                            Name = "AED10"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 20m,
                            Name = "AED20"
                        },
                        new
                        {
                            Id = 4,
                            Amount = 30m,
                            Name = "AED30"
                        },
                        new
                        {
                            Id = 5,
                            Amount = 50m,
                            Name = "AED50"
                        },
                        new
                        {
                            Id = 6,
                            Amount = 75m,
                            Name = "AED75"
                        },
                        new
                        {
                            Id = 7,
                            Amount = 100m,
                            Name = "AED100"
                        });
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsVerified = true,
                            Name = "John Doe",
                            PhoneNumber = "1234567890",
                            Remark = "This is active user for active beneficiary"
                        });
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Account", b =>
                {
                    b.HasOne("MobileTopup.Contracts.Domain.Entities.User", "User")
                        .WithOne("Account")
                        .HasForeignKey("MobileTopup.Contracts.Domain.Entities.Account", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Beneficiary", b =>
                {
                    b.HasOne("MobileTopup.Contracts.Domain.Entities.User", "User")
                        .WithMany("Beneficiaries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.TopupHistory", b =>
                {
                    b.HasOne("MobileTopup.Contracts.Domain.Entities.User", "User")
                        .WithMany("TopupHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("MobileTopup.Contracts.Domain.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("MobileTopup.Contracts.Domain.Entities.User", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();

                    b.Navigation("Beneficiaries");

                    b.Navigation("TopupHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
