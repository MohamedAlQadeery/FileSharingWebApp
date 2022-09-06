using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileSharingWeb.Data.Migrations
{
    public partial class DownloadCountColumnUploads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DownloadCount",
                table: "Uploads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Uploads");
        }
    }
}
