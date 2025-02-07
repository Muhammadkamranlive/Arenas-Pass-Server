using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddedBillingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArenasBillings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripePaymentPlanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BonusDays = table.Column<int>(type: "int", nullable: false),
                    NextPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Update_At = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArenasBillings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plan_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plan_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Billing_Cycle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trial_Period_Days = table.Column<int>(type: "int", nullable: true),
                    Max_Users = table.Column<int>(type: "int", nullable: false),
                    Max_Cards = table.Column<int>(type: "int", nullable: false),
                    Supports_Custom_Branding = table.Column<bool>(type: "bit", nullable: false),
                    Is_Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentFeatureLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeatureDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentPlanId = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFeatureLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TenantCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeType = table.Column<int>(type: "int", nullable: false),
                    ChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChargePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantCharges", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArenasBillings");

            migrationBuilder.DropTable(
                name: "Payment_Plans");

            migrationBuilder.DropTable(
                name: "PaymentFeatureLists");

            migrationBuilder.DropTable(
                name: "TenantCharges");
        }
    }
}
