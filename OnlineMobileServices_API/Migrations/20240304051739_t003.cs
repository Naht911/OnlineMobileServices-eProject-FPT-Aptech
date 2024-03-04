using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    /// <inheritdoc />
    public partial class t003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Telco",
                columns: table => new
                {
                    TelcoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelcoName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", nullable: false),
                    Logo = table.Column<string>(type: "varchar(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telco", x => x.TelcoID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistory",
                columns: table => new
                {
                    ServiceHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistory", x => x.ServiceHistoryID);
                    table.ForeignKey(
                        name: "FK_ServiceHistory_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceHistory_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "RechargePackages",
                columns: table => new
                {
                    RechargePackageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "varchar(50)", nullable: false),
                    SubscriptionCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Validity = table.Column<int>(type: "int", nullable: false),
                    DataVolume = table.Column<int>(type: "int", nullable: false),
                    VoiceCall = table.Column<int>(type: "int", nullable: false),
                    SMS = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "varchar(256)", nullable: false),
                    TelcoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RechargePackages", x => x.RechargePackageID);
                    table.ForeignKey(
                        name: "FK_RechargePackages_Telco_TelcoID",
                        column: x => x.TelcoID,
                        principalTable: "Telco",
                        principalColumn: "TelcoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialRechargePackages",
                columns: table => new
                {
                    SpecialRechargePackageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialPackageName = table.Column<string>(type: "varchar(50)", nullable: false),
                    SubscriptionCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Validity = table.Column<int>(type: "int", nullable: false),
                    DataVolume = table.Column<int>(type: "int", nullable: false),
                    VoiceCall = table.Column<int>(type: "int", nullable: false),
                    SMS = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "varchar(256)", nullable: false),
                    TelcoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRechargePackages", x => x.SpecialRechargePackageID);
                    table.ForeignKey(
                        name: "FK_SpecialRechargePackages_Telco_TelcoID",
                        column: x => x.TelcoID,
                        principalTable: "Telco",
                        principalColumn: "TelcoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RechargePackageHistories",
                columns: table => new
                {
                    RechargePackageHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RechargePackageID = table.Column<int>(type: "int", nullable: false),
                    RechargeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RechargePackageHistories", x => x.RechargePackageHistoryID);
                    table.ForeignKey(
                        name: "FK_RechargePackageHistories_RechargePackages_RechargePackageID",
                        column: x => x.RechargePackageID,
                        principalTable: "RechargePackages",
                        principalColumn: "RechargePackageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RechargePackageHistories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialRechargePackageHistories",
                columns: table => new
                {
                    SpecialRechargePackageHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobileNumber = table.Column<string>(type: "varchar(10)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    SpecialRechargePackageID = table.Column<int>(type: "int", nullable: false),
                    RechargeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(20)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRechargePackageHistories", x => x.SpecialRechargePackageHistoryID);
                    table.ForeignKey(
                        name: "FK_SpecialRechargePackageHistories_SpecialRechargePackages_SpecialRechargePackageID",
                        column: x => x.SpecialRechargePackageID,
                        principalTable: "SpecialRechargePackages",
                        principalColumn: "SpecialRechargePackageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialRechargePackageHistories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RechargePackageHistories_RechargePackageID",
                table: "RechargePackageHistories",
                column: "RechargePackageID");

            migrationBuilder.CreateIndex(
                name: "IX_RechargePackageHistories_UserID",
                table: "RechargePackageHistories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RechargePackages_TelcoID",
                table: "RechargePackages",
                column: "TelcoID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_ServiceID",
                table: "ServiceHistory",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistory_UserID",
                table: "ServiceHistory",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRechargePackageHistories_SpecialRechargePackageID",
                table: "SpecialRechargePackageHistories",
                column: "SpecialRechargePackageID");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRechargePackageHistories_UserID",
                table: "SpecialRechargePackageHistories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRechargePackages_TelcoID",
                table: "SpecialRechargePackages",
                column: "TelcoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RechargePackageHistories");

            migrationBuilder.DropTable(
                name: "ServiceHistory");

            migrationBuilder.DropTable(
                name: "SpecialRechargePackageHistories");

            migrationBuilder.DropTable(
                name: "RechargePackages");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "SpecialRechargePackages");

            migrationBuilder.DropTable(
                name: "Telco");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                });
        }
    }
}
