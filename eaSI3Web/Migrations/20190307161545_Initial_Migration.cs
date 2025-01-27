﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eaSI3Web.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JiraUserName = table.Column<string>(nullable: true),
                    SI3UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "IssuesCreation",
                columns: table => new
                {
                    IssueCreationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JiraKey = table.Column<string>(nullable: true),
                    SI3Key = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CreationResult = table.Column<int>(nullable: false),
                    CreationResultAddtionalInfo = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuesCreation", x => x.IssueCreationId);
                    table.ForeignKey(
                        name: "FK_IssuesCreation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    LoginId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    ConnectionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.LoginId);
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkTracking",
                columns: table => new
                {
                    WorkTrackingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: true),
                    TotalHours = table.Column<int>(nullable: false),
                    Week = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    TrackingDate = table.Column<DateTime>(nullable: false),
                    TrackResult = table.Column<int>(nullable: false),
                    TrackResultAddtionalInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTracking", x => x.WorkTrackingId);
                    table.ForeignKey(
                        name: "FK_WorkTracking_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssuesCreation_UserId",
                table: "IssuesCreation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_JiraUserName_SI3UserName",
                table: "Users",
                columns: new[] { "JiraUserName", "SI3UserName" },
                unique: true,
                filter: "[JiraUserName] IS NOT NULL AND [SI3UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTracking_UserId",
                table: "WorkTracking",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuesCreation");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "WorkTracking");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
