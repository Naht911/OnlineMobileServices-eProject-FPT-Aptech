using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    /// <inheritdoc />
    public partial class renameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargePackageHistories_RechargePackages_RechargePackageID",
                table: "RechargePackageHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_RechargePackageHistories_Users_UserID",
                table: "RechargePackageHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargePackageHistories_SpecialRechargePackages_SpecialRechargePackageID",
                table: "SpecialRechargePackageHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargePackageHistories_Users_UserID",
                table: "SpecialRechargePackageHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialRechargePackageHistories",
                table: "SpecialRechargePackageHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RechargePackageHistories",
                table: "RechargePackageHistories");

            migrationBuilder.RenameTable(
                name: "SpecialRechargePackageHistories",
                newName: "SpecialRechargeHistory");

            migrationBuilder.RenameTable(
                name: "RechargePackageHistories",
                newName: "RechargeHistory");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialRechargePackageHistories_UserID",
                table: "SpecialRechargeHistory",
                newName: "IX_SpecialRechargeHistory_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialRechargePackageHistories_SpecialRechargePackageID",
                table: "SpecialRechargeHistory",
                newName: "IX_SpecialRechargeHistory_SpecialRechargePackageID");

            migrationBuilder.RenameIndex(
                name: "IX_RechargePackageHistories_UserID",
                table: "RechargeHistory",
                newName: "IX_RechargeHistory_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_RechargePackageHistories_RechargePackageID",
                table: "RechargeHistory",
                newName: "IX_RechargeHistory_RechargePackageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialRechargeHistory",
                table: "SpecialRechargeHistory",
                column: "HistoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RechargeHistory",
                table: "RechargeHistory",
                column: "HistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargeHistory_RechargePackages_RechargePackageID",
                table: "RechargeHistory",
                column: "RechargePackageID",
                principalTable: "RechargePackages",
                principalColumn: "RechargePackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RechargeHistory_Users_UserID",
                table: "RechargeHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargeHistory_SpecialRechargePackages_SpecialRechargePackageID",
                table: "SpecialRechargeHistory",
                column: "SpecialRechargePackageID",
                principalTable: "SpecialRechargePackages",
                principalColumn: "SpecialRechargePackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargeHistory_Users_UserID",
                table: "SpecialRechargeHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargeHistory_RechargePackages_RechargePackageID",
                table: "RechargeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_RechargeHistory_Users_UserID",
                table: "RechargeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargeHistory_SpecialRechargePackages_SpecialRechargePackageID",
                table: "SpecialRechargeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargeHistory_Users_UserID",
                table: "SpecialRechargeHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecialRechargeHistory",
                table: "SpecialRechargeHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RechargeHistory",
                table: "RechargeHistory");

            migrationBuilder.RenameTable(
                name: "SpecialRechargeHistory",
                newName: "SpecialRechargePackageHistories");

            migrationBuilder.RenameTable(
                name: "RechargeHistory",
                newName: "RechargePackageHistories");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialRechargeHistory_UserID",
                table: "SpecialRechargePackageHistories",
                newName: "IX_SpecialRechargePackageHistories_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialRechargeHistory_SpecialRechargePackageID",
                table: "SpecialRechargePackageHistories",
                newName: "IX_SpecialRechargePackageHistories_SpecialRechargePackageID");

            migrationBuilder.RenameIndex(
                name: "IX_RechargeHistory_UserID",
                table: "RechargePackageHistories",
                newName: "IX_RechargePackageHistories_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_RechargeHistory_RechargePackageID",
                table: "RechargePackageHistories",
                newName: "IX_RechargePackageHistories_RechargePackageID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecialRechargePackageHistories",
                table: "SpecialRechargePackageHistories",
                column: "HistoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RechargePackageHistories",
                table: "RechargePackageHistories",
                column: "HistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargePackageHistories_RechargePackages_RechargePackageID",
                table: "RechargePackageHistories",
                column: "RechargePackageID",
                principalTable: "RechargePackages",
                principalColumn: "RechargePackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RechargePackageHistories_Users_UserID",
                table: "RechargePackageHistories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargePackageHistories_SpecialRechargePackages_SpecialRechargePackageID",
                table: "SpecialRechargePackageHistories",
                column: "SpecialRechargePackageID",
                principalTable: "SpecialRechargePackages",
                principalColumn: "SpecialRechargePackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargePackageHistories_Users_UserID",
                table: "SpecialRechargePackageHistories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
