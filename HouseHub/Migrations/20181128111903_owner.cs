using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseHub.Migrations
{
    public partial class owner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Accommodation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Accommodation");
        }
    }
}
