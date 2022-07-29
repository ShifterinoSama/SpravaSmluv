using Microsoft.EntityFrameworkCore.Migrations;

namespace SpravaSmluv.Migrations
{
    public partial class ErrorFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advisor_Contract_ContractID",
                table: "Advisor");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Advisor_ContractManagerID",
                table: "Contract");

            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Client_ClientID",
                table: "Contract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advisor",
                table: "Advisor");

            migrationBuilder.DropIndex(
                name: "IX_Advisor_ContractID",
                table: "Advisor");

            migrationBuilder.DropColumn(
                name: "ContractID",
                table: "Advisor");

            migrationBuilder.RenameTable(
                name: "Contract",
                newName: "Contracts");

            migrationBuilder.RenameTable(
                name: "Advisor",
                newName: "Advisors");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_ContractManagerID",
                table: "Contracts",
                newName: "IX_Contracts_ContractManagerID");

            migrationBuilder.RenameIndex(
                name: "IX_Contract_ClientID",
                table: "Contracts",
                newName: "IX_Contracts_ClientID");

            migrationBuilder.AlterColumn<string>(
                name: "EvidenceNumber",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advisors",
                table: "Advisors",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ContractAdvisors",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    AdvisorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAdvisors", x => new { x.ContractId, x.AdvisorId });
                    table.ForeignKey(
                        name: "FK_ContractAdvisors_Advisors_AdvisorId",
                        column: x => x.AdvisorId,
                        principalTable: "Advisors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractAdvisors_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractAdvisors_AdvisorId",
                table: "ContractAdvisors",
                column: "AdvisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Advisors_ContractManagerID",
                table: "Contracts",
                column: "ContractManagerID",
                principalTable: "Advisors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Client_ClientID",
                table: "Contracts",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Advisors_ContractManagerID",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Client_ClientID",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractAdvisors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contracts",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advisors",
                table: "Advisors");

            migrationBuilder.RenameTable(
                name: "Contracts",
                newName: "Contract");

            migrationBuilder.RenameTable(
                name: "Advisors",
                newName: "Advisor");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ContractManagerID",
                table: "Contract",
                newName: "IX_Contract_ContractManagerID");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_ClientID",
                table: "Contract",
                newName: "IX_Contract_ClientID");

            migrationBuilder.AlterColumn<int>(
                name: "EvidenceNumber",
                table: "Contract",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractID",
                table: "Advisor",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advisor",
                table: "Advisor",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Advisor_ContractID",
                table: "Advisor",
                column: "ContractID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advisor_Contract_ContractID",
                table: "Advisor",
                column: "ContractID",
                principalTable: "Contract",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Advisor_ContractManagerID",
                table: "Contract",
                column: "ContractManagerID",
                principalTable: "Advisor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Client_ClientID",
                table: "Contract",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
