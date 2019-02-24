﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eaSI3Web.Models;

namespace eaSI3Web.Migrations
{
    [DbContext(typeof(StatisticsContext))]
    [Migration("20190224201252_SI3UserNameAdded")]
    partial class SI3UserNameAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity("eaSI3Web.Models.IssueCreation", b =>
                {
                    b.Property<int>("IssueCreationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("CreationResult");

                    b.Property<string>("CreationResultAddtionalInfo");

                    b.Property<string>("JiraKey");

                    b.Property<string>("SI3Key");

                    b.HasKey("IssueCreationId");

                    b.ToTable("IssuesCreation");
                });

            modelBuilder.Entity("eaSI3Web.Models.Login", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ConnectionDate");

                    b.Property<int?>("UserId");

                    b.HasKey("LoginId");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("eaSI3Web.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("JiraUserName");

                    b.Property<string>("SI3UserName");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("eaSI3Web.Models.WorkTracking", b =>
                {
                    b.Property<int>("WorkTrackingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TotalHours");

                    b.Property<int>("TrackResult");

                    b.Property<string>("TrackResultAddtionalInfo");

                    b.Property<int>("TrackingDate");

                    b.Property<int?>("UserId");

                    b.Property<int>("Week");

                    b.Property<int>("Year");

                    b.HasKey("WorkTrackingId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkTracking");
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
