using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidRescue.Migrations
{
    public partial class AddEmttoDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMTs",
                columns: table => new
                {
                    EMT_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    CertificationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CertificationExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMTs", x => x.EMT_Id);
                    table.ForeignKey(
                        name: "FK_EMTs_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EMTs_User_id",
                table: "EMTs",
                column: "User_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMTs");
        }
    }
}
