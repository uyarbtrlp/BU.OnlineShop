using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BU.OnlineShop.FileService.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Size = table.Column<int>(type: "int", maxLength: 2147483647, nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", maxLength: 2147483647, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInformations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileInformations");
        }
    }
}
