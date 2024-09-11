using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Tracker_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class newProjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ProjectClientName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ProjectLink = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    ProjectType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    AwardDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecruiter = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    GrossBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CostinPKR = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AccountID",
                table: "Projects",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CurrencyId",
                table: "Projects",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
