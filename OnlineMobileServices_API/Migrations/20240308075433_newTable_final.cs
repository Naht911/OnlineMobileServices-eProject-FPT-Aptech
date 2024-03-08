using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    /// <inheritdoc />
    public partial class newTable_final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargeHistory_RechargePackages_RechargePackageID",
                table: "RechargeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargeHistory_SpecialRechargePackages_SpecialRechargePackageID",
                table: "SpecialRechargeHistory");

            migrationBuilder.RenameColumn(
                name: "SpecialRechargePackageID",
                table: "SpecialRechargeHistory",
                newName: "PackageID");

            migrationBuilder.RenameColumn(
                name: "RechargeDate",
                table: "SpecialRechargeHistory",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialRechargeHistory_SpecialRechargePackageID",
                table: "SpecialRechargeHistory",
                newName: "IX_SpecialRechargeHistory_PackageID");

            migrationBuilder.RenameColumn(
                name: "RechargePackageID",
                table: "RechargeHistory",
                newName: "PackageID");

            migrationBuilder.RenameColumn(
                name: "RechargeDate",
                table: "RechargeHistory",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_RechargeHistory_RechargePackageID",
                table: "RechargeHistory",
                newName: "IX_RechargeHistory_PackageID");

            migrationBuilder.AddColumn<int>(
                name: "RechargePackageID",
                table: "SpecialRechargeHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CallerTunesPackages",
                columns: table => new
                {
                    PackageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Validity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    Icon = table.Column<string>(type: "varchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallerTunesPackages", x => x.PackageID);
                });

            migrationBuilder.CreateTable(
                name: "DoNotDisturbHistories",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoNotDisturbHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_DoNotDisturbHistories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PostPaidHistories",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnterBillID = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPaidHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_PostPaidHistories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "CallerTunesHistories",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    PackageID = table.Column<int>(type: "int", nullable: false),
                    OriginalPackagePackageID = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallerTunesHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_CallerTunesHistories_CallerTunesPackages_OriginalPackagePackageID",
                        column: x => x.OriginalPackagePackageID,
                        principalTable: "CallerTunesPackages",
                        principalColumn: "PackageID");
                    table.ForeignKey(
                        name: "FK_CallerTunesHistories_CallerTunesPackages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "CallerTunesPackages",
                        principalColumn: "PackageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CallerTunesHistories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallerTunesHistories_OriginalPackagePackageID",
                table: "CallerTunesHistories",
                column: "OriginalPackagePackageID");

            migrationBuilder.CreateIndex(
                name: "IX_CallerTunesHistories_PackageID",
                table: "CallerTunesHistories",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_CallerTunesHistories_UserID",
                table: "CallerTunesHistories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_DoNotDisturbHistories_UserID",
                table: "DoNotDisturbHistories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PostPaidHistories_UserID",
                table: "PostPaidHistories",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargeHistory_RechargePackages_PackageID",
                table: "RechargeHistory",
                column: "PackageID",
                principalTable: "RechargePackages",
                principalColumn: "RechargePackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargeHistory_SpecialRechargePackages_PackageID",
                table: "SpecialRechargeHistory",
                column: "PackageID",
                principalTable: "SpecialRechargePackages",
                principalColumn: "SpecialRechargePackageID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RechargeHistory_RechargePackages_PackageID",
                table: "RechargeHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialRechargeHistory_SpecialRechargePackages_PackageID",
                table: "SpecialRechargeHistory");

            migrationBuilder.DropTable(
                name: "CallerTunesHistories");

            migrationBuilder.DropTable(
                name: "DoNotDisturbHistories");

            migrationBuilder.DropTable(
                name: "PostPaidHistories");

            migrationBuilder.DropTable(
                name: "CallerTunesPackages");

            migrationBuilder.DropColumn(
                name: "RechargePackageID",
                table: "SpecialRechargeHistory");

            migrationBuilder.RenameColumn(
                name: "PackageID",
                table: "SpecialRechargeHistory",
                newName: "SpecialRechargePackageID");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "SpecialRechargeHistory",
                newName: "RechargeDate");

            migrationBuilder.RenameIndex(
                name: "IX_SpecialRechargeHistory_PackageID",
                table: "SpecialRechargeHistory",
                newName: "IX_SpecialRechargeHistory_SpecialRechargePackageID");

            migrationBuilder.RenameColumn(
                name: "PackageID",
                table: "RechargeHistory",
                newName: "RechargePackageID");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "RechargeHistory",
                newName: "RechargeDate");

            migrationBuilder.RenameIndex(
                name: "IX_RechargeHistory_PackageID",
                table: "RechargeHistory",
                newName: "IX_RechargeHistory_RechargePackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_RechargeHistory_RechargePackages_RechargePackageID",
                table: "RechargeHistory",
                column: "RechargePackageID",
                principalTable: "RechargePackages",
                principalColumn: "RechargePackageID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialRechargeHistory_SpecialRechargePackages_SpecialRechargePackageID",
                table: "SpecialRechargeHistory",
                column: "SpecialRechargePackageID",
                principalTable: "SpecialRechargePackages",
                principalColumn: "SpecialRechargePackageID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
