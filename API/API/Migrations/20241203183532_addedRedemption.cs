using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addedRedemption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account_Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1291, 1"),
                    Card_Id = table.Column<int>(type: "int", nullable: false),
                    Card_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrCrFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Txn_Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tenant_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_Transactions");
        }
    }
}
