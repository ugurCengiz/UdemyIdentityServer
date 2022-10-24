using Microsoft.EntityFrameworkCore.Migrations;

namespace UdemyIdentityServer.AuthServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUser", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CustomUser",
                columns: new[] { "Id", "City", "Email", "Password", "UserName" },
                values: new object[] { 1, "İstanbul", "ugur@outlook.com", "password", "UğurCengiz" });

            migrationBuilder.InsertData(
                table: "CustomUser",
                columns: new[] { "Id", "City", "Email", "Password", "UserName" },
                values: new object[] { 2, "Ankara", "ahmet@outlook.com", "password", "Ahmet16" });

            migrationBuilder.InsertData(
                table: "CustomUser",
                columns: new[] { "Id", "City", "Email", "Password", "UserName" },
                values: new object[] { 3, "Konya", "mehmet@outlook.com", "password", "Mehmet16" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUser");
        }
    }
}
