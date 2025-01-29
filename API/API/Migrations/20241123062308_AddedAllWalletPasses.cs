using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddedAllWalletPasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "WalletPasses",
                newName: "Webiste");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Apple_Pass",
                table: "WalletPasses",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<string>(
                name: "Additional_Benefits",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AirlineCode",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AirlineName",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount_Percentage",
                table: "WalletPasses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAirportCode",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArrivalAirportName",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaggageClaimInfo",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BoardingTime",
                table: "WalletPasses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Card_Holder_Title",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Card_holder_Name",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassOfService",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code_Type",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency_Sign",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Current_Punches",
                table: "WalletPasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureAirportCode",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartureAirportName",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureTime",
                table: "WalletPasses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount_Percentage",
                table: "WalletPasses",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Effective_Date",
                table: "WalletPasses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntryGate",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrequentFlyerNumber",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GateNumber",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "Is_Redeemed",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issuer",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo_Text",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo_Url",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoyaltyCard_Reward_Details",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Member_Name",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Membership_Number",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offer",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offer_Code",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points_Balance",
                table: "WalletPasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Privacy_Policy",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Program_Name",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Punch_Title",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reward_Details",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatInfo",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Terminal",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketNumber",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ticket_Type",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Total_Punches",
                table: "WalletPasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VenueName",
                table: "WalletPasses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additional_Benefits",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "AirlineCode",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "AirlineName",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Discount_Percentage",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportCode",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "ArrivalAirportName",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "BaggageClaimInfo",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "BoardingTime",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Card_Holder_Title",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Card_holder_Name",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "ClassOfService",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Code_Type",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Currency_Sign",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Current_Punches",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "DepartureAirportCode",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "DepartureAirportName",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "DepartureTime",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Discount_Percentage",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Effective_Date",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "EntryGate",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "FrequentFlyerNumber",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "GateNumber",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "GiftCard_Currency_Code",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "GiftCard_Currency_Sign",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Is_Redeemed",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Issuer",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Logo_Text",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Logo_Url",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "LoyaltyCard_Reward_Details",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Member_Name",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Membership_Number",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Offer",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Offer_Code",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Points_Balance",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Privacy_Policy",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Program_Name",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Punch_Title",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Reward_Details",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "SeatInfo",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Terminal",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "TicketNumber",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Ticket_Type",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "Total_Punches",
                table: "WalletPasses");

            migrationBuilder.DropColumn(
                name: "VenueName",
                table: "WalletPasses");

            migrationBuilder.RenameColumn(
                name: "Webiste",
                table: "WalletPasses",
                newName: "Logo");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Apple_Pass",
                table: "WalletPasses",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}
