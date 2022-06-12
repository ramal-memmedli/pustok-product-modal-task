using Microsoft.EntityFrameworkCore.Migrations;

namespace PustokMVC.Migrations
{
    public partial class PromotionSizeColumnAddedIntoComplexPromotions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PromotionSize",
                table: "ComplexPromotions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionSize",
                table: "ComplexPromotions");
        }
    }
}
