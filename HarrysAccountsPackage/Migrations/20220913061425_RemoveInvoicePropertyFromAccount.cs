using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HarrysAccountsPackage.Migrations
{
    public partial class RemoveInvoicePropertyFromAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Accounts_AccountId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_AccountId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountId",
                table: "Invoices",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Accounts_AccountId",
                table: "Invoices",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }
    }
}
