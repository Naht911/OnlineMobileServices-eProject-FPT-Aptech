using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMobileServices_API.Migrations
{
    /// <inheritdoc />
    public partial class addWebSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsiteSettings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastEditedByID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteSettings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebsiteSettings_Users_LastEditedByID",
                        column: x => x.LastEditedByID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteSettings_LastEditedByID",
                table: "WebsiteSettings",
                column: "LastEditedByID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteSettings");
        }
    }
}
