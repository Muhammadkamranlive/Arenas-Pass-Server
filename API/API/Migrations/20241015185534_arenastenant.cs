using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class arenastenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Server.Domain.PelicanHRMTenant_Id",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "BussinessType",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "EIN",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "FillingFormIRS",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "noDomesticContractor",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "noDomesticEmployee",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "noInterContractor",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "noInterEmployee",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "whosisCompany",
                table: "PelicanHRMTenants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Server.Domain.ArenasTenants_Id",
                table: "PelicanHRMTenants",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Server.Domain.ArenasTenants_Id",
                table: "PelicanHRMTenants");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BussinessType",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EIN",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FillingFormIRS",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "PelicanHRMTenants",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "PelicanHRMTenants",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "noDomesticContractor",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "noDomesticEmployee",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "noInterContractor",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "noInterEmployee",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "whosisCompany",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Server.Domain.PelicanHRMTenant_Id",
                table: "PelicanHRMTenants",
                column: "Id");
        }
    }
}
