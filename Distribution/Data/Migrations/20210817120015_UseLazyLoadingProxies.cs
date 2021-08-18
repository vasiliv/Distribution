using Microsoft.EntityFrameworkCore.Migrations;

namespace Distribution.Data.Migrations
{
    public partial class UseLazyLoadingProxies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Record",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Record");
        }
    }
}
