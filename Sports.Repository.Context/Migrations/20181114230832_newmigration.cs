using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Repository.Context.Migrations
{
    public partial class newmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Competitors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Competitors",
                nullable: false,
                defaultValue: 0);
        }
    }
}
