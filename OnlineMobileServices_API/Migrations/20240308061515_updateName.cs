using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    /// <inheritdoc />
    public partial class updateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargePackageHistories_Users_UserID",
                table: "RechargePackageHistories");

            migrationBuilder.RenameColumn(
                name: "SpecialRechargePackageHistoryID",
                table: "SpecialRechargePackageHistories",
                newName: "HistoryID");

            migrationBuilder.RenameColumn(
                name: "RechargePackageHistoryID",
                table: "RechargePackageHistories",
                newName: "HistoryID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "RechargePackageHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargePackageHistories_Users_UserID",
                table: "RechargePackageHistories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargePackageHistories_Users_UserID",
                table: "RechargePackageHistories");

            migrationBuilder.RenameColumn(
                name: "HistoryID",
                table: "SpecialRechargePackageHistories",
                newName: "SpecialRechargePackageHistoryID");

            migrationBuilder.RenameColumn(
                name: "HistoryID",
                table: "RechargePackageHistories",
                newName: "RechargePackageHistoryID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "RechargePackageHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RechargePackageHistories_Users_UserID",
                table: "RechargePackageHistories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
