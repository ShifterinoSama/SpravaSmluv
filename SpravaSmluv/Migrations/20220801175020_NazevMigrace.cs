using Microsoft.EntityFrameworkCore.Migrations;

namespace SpravaSmluv.Migrations
{
    public partial class NazevMigrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractManagerId",
                table: "Contracts");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractManagerId",
                table: "Contracts",
                column: "ContractManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractManagerId",
                table: "Contracts");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractManagerId",
                table: "Contracts",
                column: "ContractManagerId",
                unique: true);
        }
    }
}
