﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eaSI3Web.Models;

namespace eaSI3Web.Migrations
{
    [DbContext(typeof(StatisticsContext))]
    [Migration("20191220092312_CodUserAdded")]
    partial class CodUserAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("eaSI3Web.Models.IssueCreation", b =>
                {
                    b.Property<int>("IssueCreationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("CreationResult");

                    b.Property<string>("CreationResultAddtionalInfo");

                    b.Property<string>("JiraKey");

                    b.Property<string>("SI3Key");

                    b.Property<int?>("UserId");

                    b.HasKey("IssueCreationId");

                    b.HasIndex("UserId");

                    b.ToTable("IssuesCreation");
                });

            modelBuilder.Entity("eaSI3Web.Models.Login", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ConnectionDate");

                    b.Property<int?>("UserId");

                    b.HasKey("LoginId");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("eaSI3Web.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CodUser");

                    b.Property<string>("JiraUserName");

                    b.Property<byte[]>("PasswordSi3_Encrypted");

                    b.Property<byte[]>("Password_Encrypted");

                    b.Property<string>("SI3UserName");

                    b.HasKey("UserId");

                    b.HasIndex("JiraUserName", "SI3UserName")
                        .IsUnique()
                        .HasFilter("[JiraUserName] IS NOT NULL AND [SI3UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("eaSI3Web.Models.WorkTracking", b =>
                {
                    b.Property<int>("WorkTrackingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Submit");

                    b.Property<int>("TotalHours");

                    b.Property<int>("TrackResult");

                    b.Property<string>("TrackResultAddtionalInfo");

                    b.Property<DateTime>("TrackingDate");

                    b.Property<int?>("UserId");

                    b.Property<int>("Week");

                    b.Property<int>("Year");

                    b.HasKey("WorkTrackingId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkTracking");
                });

            modelBuilder.Entity("eaSI3Web.Models.IssueCreation", b =>
                {
                    b.HasOne("eaSI3Web.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("eaSI3Web.Models.Login", b =>
                {
                    b.HasOne("eaSI3Web.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("eaSI3Web.Models.WorkTracking", b =>
                {
                    b.HasOne("eaSI3Web.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
