using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Tracker_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class payslipupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_AspNetUsers_UserId1",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_UserId1",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Payslips");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Payslips",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_UserId",
                table: "Payslips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_AspNetUsers_UserId",
                table: "Payslips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_AspNetUsers_UserId",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_UserId",
                table: "Payslips");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payslips",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Payslips",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_UserId1",
                table: "Payslips",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_AspNetUsers_UserId1",
                table: "Payslips",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
