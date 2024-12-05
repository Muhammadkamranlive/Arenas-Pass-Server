using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddedIdandUsernameToUserVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uploaded_By",
                table: "UserVouchers",
                newName: "UploadedByUsername");

            migrationBuilder.AddColumn<string>(
                name: "ProcessedByUserId",
                table: "UserVouchers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessedByUsername",
                table: "UserVouchers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploadedByUserId",
                table: "UserVouchers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedByUserId",
                table: "UserVouchers");

            migrationBuilder.DropColumn(
                name: "ProcessedByUsername",
                table: "UserVouchers");

            migrationBuilder.DropColumn(
                name: "UploadedByUserId",
                table: "UserVouchers");

            migrationBuilder.RenameColumn(
                name: "UploadedByUsername",
                table: "UserVouchers",
                newName: "Uploaded_By");
        }
    }
}
