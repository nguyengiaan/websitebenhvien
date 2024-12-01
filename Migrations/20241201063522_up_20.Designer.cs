﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using websitebenhvien.Data;

#nullable disable

namespace websitebenhvien.Migrations
{
    [DbContext(typeof(MyDbcontext))]
    [Migration("20241201063522_up_20")]
    partial class up_20
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Categorynews", b =>
                {
                    b.Property<string>("Id_Categorynews")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Alias_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Createat")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Categorynews");

                    b.ToTable("Categorynews", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Categoryproduct", b =>
                {
                    b.Property<string>("Id_Categoryproduct")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Alias_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Createat")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Categoryproduct");

                    b.ToTable("Categoryproduct", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Footer", b =>
                {
                    b.Property<string>("Id_footer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkcdha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkdn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkdnbs")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkface")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkgt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkkb")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linknt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linktd")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linktn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linktwiter")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkxn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("linkyoutube")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_footer");

                    b.ToTable("Footer", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Header", b =>
                {
                    b.Property<string>("Id_header")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_header");

                    b.ToTable("Header", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Logocustomer", b =>
                {
                    b.Property<string>("Id_logocustomer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Attime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id_logocustomer");

                    b.ToTable("Logocustomer", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Menu", b =>
                {
                    b.Property<string>("Id_menu")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link_menu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order_menu")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title_menu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_menu");

                    b.ToTable("Menu", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Menuchild", b =>
                {
                    b.Property<string>("Id_MenuChild")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id_menu")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Link_MenuChild")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order_menu")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Title_MenuChild")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_MenuChild");

                    b.HasIndex("Id_menu");

                    b.ToTable("MenuChild", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.News", b =>
                {
                    b.Property<string>("Id_News")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Alias_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Createat")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id_Categorynews")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_News");

                    b.HasIndex("Id_Categorynews");

                    b.ToTable("News", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Product", b =>
                {
                    b.Property<string>("Id_product")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Alias_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Createat")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id_Categoryproduct")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Product_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_product");

                    b.HasIndex("Id_Categoryproduct");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Proimages", b =>
                {
                    b.Property<string>("Id_proimages")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id_product")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_proimages");

                    b.HasIndex("Id_product");

                    b.ToTable("Proimages", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Sharecustomer", b =>
                {
                    b.Property<string>("Id_share")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Createat")
                        .HasColumnType("datetime2");

                    b.Property<string>("aliasurl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("status")
                        .HasColumnType("bit");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_share");

                    b.ToTable("Sharecustomer", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Slidepage", b =>
                {
                    b.Property<string>("Id_slidepage")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("slide")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sort")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_slidepage");

                    b.ToTable("Slidepage", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.TimeWork", b =>
                {
                    b.Property<string>("Id_timework")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_timework");

                    b.ToTable("TimeWork", (string)null);
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Titleheader", b =>
                {
                    b.Property<string>("Id_titleheader")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Id_header")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_titleheader");

                    b.HasIndex("Id_header");

                    b.ToTable("Titleheader", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("websitebenhvien.Models.Enitity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Menuchild", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.Menu", "Menu")
                        .WithMany("Menuchilds")
                        .HasForeignKey("Id_menu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.News", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.Categorynews", "Categorynews")
                        .WithMany("News")
                        .HasForeignKey("Id_Categorynews")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorynews");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Product", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.Categoryproduct", "Categoryproduct")
                        .WithMany("Products")
                        .HasForeignKey("Id_Categoryproduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoryproduct");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Proimages", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.Product", "Product")
                        .WithMany("Proimages")
                        .HasForeignKey("Id_product")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Titleheader", b =>
                {
                    b.HasOne("websitebenhvien.Models.Enitity.Header", "Header")
                        .WithMany("titleheader")
                        .HasForeignKey("Id_header")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Header");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Categorynews", b =>
                {
                    b.Navigation("News");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Categoryproduct", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Header", b =>
                {
                    b.Navigation("titleheader");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Menu", b =>
                {
                    b.Navigation("Menuchilds");
                });

            modelBuilder.Entity("websitebenhvien.Models.Enitity.Product", b =>
                {
                    b.Navigation("Proimages");
                });
#pragma warning restore 612, 618
        }
    }
}
