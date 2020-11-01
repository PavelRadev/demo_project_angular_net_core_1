using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DB.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedByUserId = table.Column<Guid>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    HashedPassword = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    IsGlobalAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyName", "DeletedAt", "DeletedByUserId", "Email", "FirstName", "HashedPassword", "IsGlobalAdmin", "LastName" },
                values: new object[] { new Guid("914a2250-fe53-4a65-95d3-8edc6e079884"), "TestCompany", null, null, "main@mailinator.com", "Root", "AQAAAAEAACcQAAAAEAJKeZ4fD8jinehtYPskwJ6Wuau1g1T3e7xyt+3uUyiASHipi9JY0X5hcy+i7RWmZg==", true, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedByUserId",
                table: "Users",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
