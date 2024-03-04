using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    /// <inheritdoc />
    public partial class t004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargePackages_Telco_TelcoID",
                table: "RechargePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistory_Services_ServiceID",
                table: "ServiceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistory_Users_UserID",
                table: "ServiceHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargePackages_Telco_TelcoID",
                table: "SpecialRechargePackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Telco",
                table: "Telco");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceHistory",
                table: "ServiceHistory");

            migrationBuilder.RenameTable(
                name: "Telco",
                newName: "Telcos");

            migrationBuilder.RenameTable(
                name: "ServiceHistory",
                newName: "ServiceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceHistory_UserID",
                table: "ServiceHistories",
                newName: "IX_ServiceHistories_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceHistory_ServiceID",
                table: "ServiceHistories",
                newName: "IX_ServiceHistories_ServiceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Telcos",
                table: "Telcos",
                column: "TelcoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceHistories",
                table: "ServiceHistories",
                column: "ServiceHistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargePackages_Telcos_TelcoID",
                table: "RechargePackages",
                column: "TelcoID",
                principalTable: "Telcos",
                principalColumn: "TelcoID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistories_Services_ServiceID",
                table: "ServiceHistories",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistories_Users_UserID",
                table: "ServiceHistories",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargePackages_Telcos_TelcoID",
                table: "SpecialRechargePackages",
                column: "TelcoID",
                principalTable: "Telcos",
                principalColumn: "TelcoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargePackages_Telcos_TelcoID",
                table: "RechargePackages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistories_Services_ServiceID",
                table: "ServiceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistories_Users_UserID",
                table: "ServiceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargePackages_Telcos_TelcoID",
                table: "SpecialRechargePackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Telcos",
                table: "Telcos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceHistories",
                table: "ServiceHistories");

            migrationBuilder.RenameTable(
                name: "Telcos",
                newName: "Telco");

            migrationBuilder.RenameTable(
                name: "ServiceHistories",
                newName: "ServiceHistory");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceHistories_UserID",
                table: "ServiceHistory",
                newName: "IX_ServiceHistory_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceHistories_ServiceID",
                table: "ServiceHistory",
                newName: "IX_ServiceHistory_ServiceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Telco",
                table: "Telco",
                column: "TelcoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceHistory",
                table: "ServiceHistory",
                column: "ServiceHistoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargePackages_Telco_TelcoID",
                table: "RechargePackages",
                column: "TelcoID",
                principalTable: "Telco",
                principalColumn: "TelcoID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistory_Services_ServiceID",
                table: "ServiceHistory",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistory_Users_UserID",
                table: "ServiceHistory",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargePackages_Telco_TelcoID",
                table: "SpecialRechargePackages",
                column: "TelcoID",
                principalTable: "Telco",
                principalColumn: "TelcoID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
