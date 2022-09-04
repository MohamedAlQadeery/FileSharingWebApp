using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSharingWeb.Data.Migrations
{
    public partial class UploadUrlAndPublicId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uploadUrl",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "uploadUrl",
                table: "Uploads");
        }
    }
}
