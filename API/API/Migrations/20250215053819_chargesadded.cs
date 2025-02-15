using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class chargesadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TenantCharges");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "TenantCharges");

            migrationBuilder.RenameColumn(
                name: "CharName",
                table: "TenantCharges",
                newName: "ChargeName");

            migrationBuilder.AlterColumn<string>(
                name: "ChargeType",
                table: "TenantCharges",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChargeName",
                table: "TenantCharges",
                newName: "CharName");

            migrationBuilder.AlterColumn<int>(
                name: "ChargeType",
                table: "TenantCharges",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TenantCharges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "TenantCharges",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
