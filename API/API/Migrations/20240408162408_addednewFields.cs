using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addednewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "PelicanHRMTenants");
        }
    }
}
