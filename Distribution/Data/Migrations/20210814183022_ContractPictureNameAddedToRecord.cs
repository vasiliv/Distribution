using Microsoft.EntityFrameworkCore.Migrations;

namespace Distribution.Data.Migrations
{
    public partial class ContractPictureNameAddedToRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractPictureName",
                table: "Record",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractPictureName",
                table: "Record");
        }
    }
}
