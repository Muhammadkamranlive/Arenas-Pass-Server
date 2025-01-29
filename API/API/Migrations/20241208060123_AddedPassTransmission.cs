using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddedPassTransmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Txn_Type",
                table: "Account_Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Account_Balance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tenant_Id = table.Column<int>(type: "int", nullable: false),
                    ACCOUNT_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Process_By = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Txn_Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Balance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pass_Transmission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Card_Id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pass_Trans_Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pass_Transmission", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_Balance");

            migrationBuilder.DropTable(
                name: "Pass_Transmission");

            migrationBuilder.DropColumn(
                name: "Txn_Type",
                table: "Account_Transactions");
        }
    }
}
