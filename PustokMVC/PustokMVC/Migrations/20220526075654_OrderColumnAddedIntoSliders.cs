﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PustokMVC.Migrations
{
    public partial class OrderColumnAddedIntoSliders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Sliders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Sliders");
        }
    }
}
