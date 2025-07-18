using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuctionProductTablev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Winnings_AuctionProductId",
                table: "Winnings");

            migrationBuilder.CreateIndex(
                name: "IX_Winnings_AuctionProductId",
                table: "Winnings",
                column: "AuctionProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Winnings_AuctionProductId",
                table: "Winnings");

            migrationBuilder.CreateIndex(
                name: "IX_Winnings_AuctionProductId",
                table: "Winnings",
                column: "AuctionProductId");
        }
    }
}
