using Microsoft.EntityFrameworkCore.Migrations;

namespace MemberAPI.Data.Migrations
{
    public partial class TokenColumnsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "Member",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForgotPasswordConfirmationToken",
                table: "Member",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "Member",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ForgotPasswordConfirmationToken",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "Member");
        }
    }
}
