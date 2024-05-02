using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLiteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class FixSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "InvoiceDetails",
                comment: "Invoice details");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "InvoiceDetails",
                type: "INTEGER",
                nullable: false,
                comment: "Id of invoice detail",
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "InvoiceDetails",
                oldComment: "Invoice details");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "InvoiceDetails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldComment: "Id of invoice detail");
        }
    }
}
