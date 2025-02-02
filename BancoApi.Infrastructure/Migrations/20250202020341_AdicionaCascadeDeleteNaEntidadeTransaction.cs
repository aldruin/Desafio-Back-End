using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCascadeDeleteNaEntidadeTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallet_DestineWalletId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallet_OriginWalletId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallet_DestineWalletId",
                table: "Transaction",
                column: "DestineWalletId",
                principalTable: "Wallet",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallet_OriginWalletId",
                table: "Transaction",
                column: "OriginWalletId",
                principalTable: "Wallet",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallet_DestineWalletId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallet_OriginWalletId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallet_DestineWalletId",
                table: "Transaction",
                column: "DestineWalletId",
                principalTable: "Wallet",
                principalColumn: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallet_OriginWalletId",
                table: "Transaction",
                column: "OriginWalletId",
                principalTable: "Wallet",
                principalColumn: "WalletId");
        }
    }
}
