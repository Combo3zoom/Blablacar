﻿// <auto-generated />
using System;
using Blabalacar.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blabalacar.Migrations
{
    [DbContext(typeof(BlalacarContext))]
    [Migration("20220915175023_updateDateTime")]
    partial class updateDateTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Blabalacar.Models.Auto.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("Expires")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Blabalacar.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("EndRoute")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StartRoute")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Route");
                });

            modelBuilder.Entity("Blabalacar.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DepartureAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TripCreatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("Trip");
                });

            modelBuilder.Entity("Blabalacar.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsVerification")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("TokenCreated")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("TokenExpires")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("UserCreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Blabalacar.Models.UserTrip", b =>
                {
                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TripId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTrips");
                });

            modelBuilder.Entity("Blabalacar.Models.Trip", b =>
                {
                    b.HasOne("Blabalacar.Models.Route", "Route")
                        .WithMany("Trips")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");
                });

            modelBuilder.Entity("Blabalacar.Models.UserTrip", b =>
                {
                    b.HasOne("Blabalacar.Models.Trip", "Trip")
                        .WithMany("UserTrips")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blabalacar.Models.User", "User")
                        .WithMany("UserTrips")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blabalacar.Models.Route", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("Blabalacar.Models.Trip", b =>
                {
                    b.Navigation("UserTrips");
                });

            modelBuilder.Entity("Blabalacar.Models.User", b =>
                {
                    b.Navigation("UserTrips");
                });
#pragma warning restore 612, 618
        }
    }
}
