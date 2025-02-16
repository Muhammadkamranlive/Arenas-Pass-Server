using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class updatedcargesmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargePercentage",
                table: "TenantCharges");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "TenantCharges");

            migrationBuilder.DropColumn(
                name: "MaxChargeAmount",
                table: "TenantCharges");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChargePercentage",
                table: "TenantCharges",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "TenantCharges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxChargeAmount",
                table: "TenantCharges",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
