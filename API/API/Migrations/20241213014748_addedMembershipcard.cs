using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class addedMembershipcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiftCard_Currency_Code",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "GiftCard_Currency_Sign",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Offer",
                table: "WalletPasses");

          
            migrationBuilder.AddColumn<decimal>(
                name: "Coupon_Discount_Percentage",
                table: "WalletPasses",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coupon_Discount_Percentage",
                table: "WalletPasses");

            migrationBuilder.AddColumn<string>(
                name: "GiftCard_Currency_Code",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GiftCard_Currency_Sign",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offer",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
