using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addednewMiddle12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the existing EmployeeId column
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

              migrationBuilder.AddColumn<int>(
              name: "EmployeeId",
              table: "AspNetUsers",
              type: "int",
              nullable: false);
               
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the newly added EmployeeId column
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            // Add back the original EmployeeId column (if necessary)
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false);
        }


    }
}
