using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeErroDeRelacionamentoComTransactionEWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallet_DestineWalletId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_DestineWalletId",
                table: "Transaction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestineWalletId",
                table: "Transaction",
                column: "DestineWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallet_DestineWalletId",
                table: "Transaction",
                column: "DestineWalletId",
                principalTable: "Wallet",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
