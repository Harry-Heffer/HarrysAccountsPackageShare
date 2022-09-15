using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarrysAccountsPackage.Migrations
{
    public partial class InvoicesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Invoices");
        }
    }
}
