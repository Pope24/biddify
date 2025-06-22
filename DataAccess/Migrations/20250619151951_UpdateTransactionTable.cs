using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionCode",
                table: "Transactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionCode",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Transactions",
                type: "text",
                nullable: true);
        }
    }
}
