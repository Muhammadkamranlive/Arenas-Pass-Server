using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addednewMiddle13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "EmployeeId",
               table: "AspNetUsers");

            // Manually update the database schema to remove the IDENTITY property from the EmployeeId column

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "EmployeeId",
               table: "AspNetUsers");

            // Create a new migration to add the EmployeeId column back with the IDENTITY property
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");
        }

    }
}
