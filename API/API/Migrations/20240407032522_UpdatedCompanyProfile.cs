using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdatedCompanyProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BussinessType",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EIN",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FillingFormIRS",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isMailingAddress",
                table: "PelicanHRMTenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPhysicalAddress",
                table: "PelicanHRMTenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "noDomesticContractor",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "noDomesticEmployee",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "noInterContractor",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "noInterEmployee",
                table: "PelicanHRMTenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "whosisCompany",
                table: "PelicanHRMTenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "BussinessType",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "City",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "Country",
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
                name: "LegalName",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "State",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "isMailingAddress",
                table: "PelicanHRMTenants");

            migrationBuilder.DropColumn(
                name: "isPhysicalAddress",
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
        }
    }
}
