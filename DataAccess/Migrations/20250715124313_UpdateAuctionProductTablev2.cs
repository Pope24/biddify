using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuctionProductTablev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WinnerId",
                table: "AuctionProducts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionProducts_WinnerId",
                table: "AuctionProducts",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionProducts_AspNetUsers_WinnerId",
                table: "AuctionProducts",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionProducts_AspNetUsers_WinnerId",
                table: "AuctionProducts");

            migrationBuilder.DropIndex(
                name: "IX_AuctionProducts_WinnerId",
                table: "AuctionProducts");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "AuctionProducts");
        }
    }
}
