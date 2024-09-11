using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Tracker_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class removeprojecttables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projacts");

            migrationBuilder.DropTable(
                name: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projacts",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CostinPKR = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrossBudget = table.Column<double>(type: "float", nullable: false),
                    NetBudget = table.Column<double>(type: "float", nullable: false),
                    ProjectClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isRecruiter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projacts", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projacts_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Bids_BidId",
                        column: x => x.BidId,
                        principalTable: "Bids",
                        principalColumn: "BidId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projacts_AccountID",
                table: "Projacts",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Projacts_UserId",
                table: "Projacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_BidId",
                table: "Projects",
                column: "BidId");
        }
    }
}
