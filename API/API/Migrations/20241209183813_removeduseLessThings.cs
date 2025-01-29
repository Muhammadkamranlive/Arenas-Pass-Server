using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class removeduseLessThings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apple_Pass",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Authentication_Token",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Barcode_Format",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Barcode_Type",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Effective_Date",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Relevant_Date",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Web_Service_URL",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Webiste",
                table: "WalletPasses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Apple_Pass",
                table: "WalletPasses",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Authentication_Token",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode_Format",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode_Type",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Effective_Date",
                table: "WalletPasses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Relevant_Date",
                table: "WalletPasses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Web_Service_URL",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Webiste",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
