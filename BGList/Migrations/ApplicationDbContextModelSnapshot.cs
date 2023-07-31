﻿// <auto-generated />
using System;
using BGList.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BGList.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("BGList.Model.BoardGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BGGRank")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ComplexityAverage")
                        .HasPrecision(4, 2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinAge")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinPlayers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnedUsers")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayTime")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("RatingAverage")
                        .HasPrecision(4, 2)
                        .HasColumnType("TEXT");

                    b.Property<int>("UsersRated")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("BoardGames");
                });

            modelBuilder.Entity("BGList.Model.BoardGames_Domains", b =>
                {
                    b.Property<int>("BoardGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DomainId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.HasKey("BoardGameId", "DomainId");

                    b.HasIndex("DomainId");

                    b.ToTable("BoardGames_Domains");
                });

            modelBuilder.Entity("BGList.Model.BoardGames_Mechanics", b =>
                {
                    b.Property<int>("BoardGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MechanicId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.HasKey("BoardGameId", "MechanicId");

                    b.HasIndex("MechanicId");

                    b.ToTable("BoardGames_Mechanics");
                });

            modelBuilder.Entity("BGList.Model.Domain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Domains");
                });

            modelBuilder.Entity("BGList.Model.Mechanic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Mechanics");
                });

            modelBuilder.Entity("BGList.Model.BoardGames_Domains", b =>
                {
                    b.HasOne("BGList.Model.BoardGame", "BoardGame")
                        .WithMany("BoardGames_Domains")
                        .HasForeignKey("BoardGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGList.Model.Domain", "Domain")
                        .WithMany("BoardGames_Domains")
                        .HasForeignKey("DomainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardGame");

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("BGList.Model.BoardGames_Mechanics", b =>
                {
                    b.HasOne("BGList.Model.BoardGame", "BoardGame")
                        .WithMany("BoardGames_Mechanics")
                        .HasForeignKey("BoardGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BGList.Model.Mechanic", "Mechanic")
                        .WithMany("BoardGames_Mechanics")
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardGame");

                    b.Navigation("Mechanic");
                });

            modelBuilder.Entity("BGList.Model.BoardGame", b =>
                {
                    b.Navigation("BoardGames_Domains");

                    b.Navigation("BoardGames_Mechanics");
                });

            modelBuilder.Entity("BGList.Model.Domain", b =>
                {
                    b.Navigation("BoardGames_Domains");
                });

            modelBuilder.Entity("BGList.Model.Mechanic", b =>
                {
                    b.Navigation("BoardGames_Mechanics");
                });
#pragma warning restore 612, 618
        }
    }
}