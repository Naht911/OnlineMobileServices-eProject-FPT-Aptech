﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineMobileServices_API.Models;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240306205206_updateUser_04")]
    partial class updateUser_04
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineMobileServices_Models.Models.RechargePackage", b =>
                {
                    b.Property<int>("RechargePackageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RechargePackageID"));

                    b.Property<int>("DataVolume")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PackageName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SMS")
                        .HasColumnType("int");

                    b.Property<string>("SubscriptionCode")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("TelcoID")
                        .HasColumnType("int");

                    b.Property<int>("Validity")
                        .HasColumnType("int");

                    b.Property<int>("VoiceCall")
                        .HasColumnType("int");

                    b.HasKey("RechargePackageID");

                    b.HasIndex("TelcoID");

                    b.ToTable("RechargePackages");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.RechargePackageHistory", b =>
                {
                    b.Property<int>("RechargePackageHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RechargePackageHistoryID"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("RechargeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RechargePackageID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("RechargePackageHistoryID");

                    b.HasIndex("RechargePackageID");

                    b.HasIndex("UserID");

                    b.ToTable("RechargePackageHistories");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.ServiceHistory", b =>
                {
                    b.Property<int>("ServiceHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceHistoryID"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("ServiceDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ServiceID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ServiceHistoryID");

                    b.HasIndex("ServiceID");

                    b.HasIndex("UserID");

                    b.ToTable("ServiceHistories");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.Services", b =>
                {
                    b.Property<int>("ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceID"));

                    b.Property<string>("Description")
                        .HasColumnType("varchar(512)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("ServiceID");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.SpecialRechargePackage", b =>
                {
                    b.Property<int>("SpecialRechargePackageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpecialRechargePackageID"));

                    b.Property<int>("DataVolume")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SMS")
                        .HasColumnType("int");

                    b.Property<string>("SpecialPackageName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SubscriptionCode")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("TelcoID")
                        .HasColumnType("int");

                    b.Property<int>("Validity")
                        .HasColumnType("int");

                    b.Property<int>("VoiceCall")
                        .HasColumnType("int");

                    b.HasKey("SpecialRechargePackageID");

                    b.HasIndex("TelcoID");

                    b.ToTable("SpecialRechargePackages");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.SpecialRechargePackageHistory", b =>
                {
                    b.Property<int>("SpecialRechargePackageHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpecialRechargePackageHistoryID"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("RechargeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SpecialRechargePackageID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("SpecialRechargePackageHistoryID");

                    b.HasIndex("SpecialRechargePackageID");

                    b.HasIndex("UserID");

                    b.ToTable("SpecialRechargePackageHistories");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.Telco", b =>
                {
                    b.Property<int>("TelcoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TelcoID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("varchar(256)");

                    b.Property<string>("TelcoName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("TelcoID");

                    b.ToTable("Telcos");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("FullName")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .HasColumnType("varchar(20)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.RechargePackage", b =>
                {
                    b.HasOne("OnlineMobileServices_Models.Models.Telco", "Telco")
                        .WithMany()
                        .HasForeignKey("TelcoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Telco");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.RechargePackageHistory", b =>
                {
                    b.HasOne("OnlineMobileServices_Models.Models.RechargePackage", "RechargePackage")
                        .WithMany("RechargePackageHistories")
                        .HasForeignKey("RechargePackageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineMobileServices_Models.Models.User", "User")
                        .WithMany("RechargePackageHistories")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RechargePackage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.ServiceHistory", b =>
                {
                    b.HasOne("OnlineMobileServices_Models.Models.Services", "Service")
                        .WithMany("ServiceHistories")
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineMobileServices_Models.Models.User", "User")
                        .WithMany("ServiceHistories")
                        .HasForeignKey("UserID");

                    b.Navigation("Service");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.SpecialRechargePackage", b =>
                {
                    b.HasOne("OnlineMobileServices_Models.Models.Telco", "Telco")
                        .WithMany()
                        .HasForeignKey("TelcoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Telco");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.SpecialRechargePackageHistory", b =>
                {
                    b.HasOne("OnlineMobileServices_Models.Models.SpecialRechargePackage", "SpecialRechargePackage")
                        .WithMany("SpecialRechargePackageHistories")
                        .HasForeignKey("SpecialRechargePackageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineMobileServices_Models.Models.User", "User")
                        .WithMany("SpecialRechargePackageHistories")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SpecialRechargePackage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.RechargePackage", b =>
                {
                    b.Navigation("RechargePackageHistories");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.Services", b =>
                {
                    b.Navigation("ServiceHistories");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.SpecialRechargePackage", b =>
                {
                    b.Navigation("SpecialRechargePackageHistories");
                });

            modelBuilder.Entity("OnlineMobileServices_Models.Models.User", b =>
                {
                    b.Navigation("RechargePackageHistories");

                    b.Navigation("ServiceHistories");

                    b.Navigation("SpecialRechargePackageHistories");
                });
#pragma warning restore 612, 618
        }
    }
}