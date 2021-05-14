using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMishnahADay.Migrations
{
    public partial class AddSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MishnahID = table.Column<int>(type: "INTEGER", nullable: false),
                    StartOnSystemStartup = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeForToast = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Settings_Mishnayos_MishnahID",
                        column: x => x.MishnahID,
                        principalTable: "Mishnayos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "ID", "MishnahID", "StartOnSystemStartup", "TimeForToast" },
                values: new object[] { 1, 1, true, new DateTime(1970, 1, 1, 15, 30, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_MishnahID",
                table: "Settings",
                column: "MishnahID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
