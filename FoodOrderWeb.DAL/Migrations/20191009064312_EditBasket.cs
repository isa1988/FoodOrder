using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodOrderWeb.DAL.Migrations
{
    public partial class EditBasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Baskets",
                newName: "Sum");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAction",
                table: "Baskets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Baskets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_OrganizationId",
                table: "Baskets",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Organizations_OrganizationId",
                table: "Baskets",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Organizations_OrganizationId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_OrganizationId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "DateAction",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Baskets");

            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "Baskets",
                newName: "Price");
        }
    }
}
