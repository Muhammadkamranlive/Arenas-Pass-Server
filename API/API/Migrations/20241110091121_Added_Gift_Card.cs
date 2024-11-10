using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class Added_Gift_Card : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "WalletPasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Background_Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foreground_Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label_Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organization_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localized_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Terms_And_Conditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Web_Service_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Authentication_Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apple_Pass = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Currency_Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipient_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode_Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode_Format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiration_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Relevant_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletPasses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletPasses");

            
        }
    }
}
