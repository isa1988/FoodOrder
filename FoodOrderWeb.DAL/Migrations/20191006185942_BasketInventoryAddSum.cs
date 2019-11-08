using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodOrderWeb.DAL.Migrations
{
    public partial class BasketInventoryAddSum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Sum",
                table: "BasketInventories",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sum",
                table: "BasketInventories");
        }
    }
}
