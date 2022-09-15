using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarrysAccountsPackage.Migrations
{
    public partial class AddTermsTypePropertyToAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TermsType",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TermsType",
                table: "Accounts");
        }
    }
}
