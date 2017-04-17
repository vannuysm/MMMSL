﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using mmmsl.Models;

namespace mmmsl.Migrations
{
    [DbContext(typeof(MmmslDatabase))]
    partial class MmmslDatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("mmmsl.Models.Division", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("mmmsl.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayTeamId");

                    b.Property<DateTimeOffset>("DateAndTime");

                    b.Property<string>("DivisionId")
                        .IsRequired();

                    b.Property<int>("HomeTeamId");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("mmmsl.Models.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("Goals");
                });

            modelBuilder.Entity("mmmsl.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("mmmsl.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DivisionId")
                        .IsRequired();

                    b.Property<int>("ManagerId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("DivisionId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("mmmsl.Models.Game", b =>
                {
                    b.HasOne("mmmsl.Models.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("mmmsl.Models.Division", "Division")
                        .WithMany("Games")
                        .HasForeignKey("DivisionId");

                    b.HasOne("mmmsl.Models.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");
                });

            modelBuilder.Entity("mmmsl.Models.Goal", b =>
                {
                    b.HasOne("mmmsl.Models.Game", "Game")
                        .WithMany("Goals")
                        .HasForeignKey("GameId");

                    b.HasOne("mmmsl.Models.Profile", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("mmmsl.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("mmmsl.Models.Team", b =>
                {
                    b.HasOne("mmmsl.Models.Division", "Division")
                        .WithMany("Teams")
                        .HasForeignKey("DivisionId");

                    b.HasOne("mmmsl.Models.Profile", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");
                });
        }
    }
}