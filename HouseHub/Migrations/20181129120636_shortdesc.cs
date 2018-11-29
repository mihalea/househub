using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseHub.Migrations
{
    public partial class shortdesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Accommodation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Accommodation");
        }
    }
}
