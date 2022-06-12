using Microsoft.EntityFrameworkCore.Migrations;

namespace PustokMVC.Migrations
{
    public partial class AuthorsTableModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Authors_AuthorId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_AuthorId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Authors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorId",
                table: "Authors",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Authors_AuthorId",
                table: "Authors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
