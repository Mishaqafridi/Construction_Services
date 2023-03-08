using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction_Admin_Service.Migrations
{
    /// <inheritdoc />
    public partial class addcontractor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    ContractorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractorAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.ContractorId);
                });

            migrationBuilder.CreateTable(
                name: "ContractorQuotations",
                columns: table => new
                {
                    QuotationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotatinDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quotationstatus = table.Column<bool>(type: "bit", nullable: false),
                    Amountstatus = table.Column<bool>(type: "bit", nullable: false),
                    ContractorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorQuotations", x => x.QuotationID);
                    table.ForeignKey(
                        name: "FK_ContractorQuotations_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "ContractorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractorQuotations_ContractorId",
                table: "ContractorQuotations",
                column: "ContractorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorQuotations");

            migrationBuilder.DropTable(
                name: "Contractors");
        }
    }
}
