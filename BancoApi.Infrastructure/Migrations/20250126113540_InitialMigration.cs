using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Chave Primária do Usuário"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Nome do Usuário"),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Sobrenome do Usuário"),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false, comment: "CPF do Usuário"),
                    Email = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Chave Primária da Carteira"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Chave Estrangeira para o Usuário"),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false, comment: "Saldo da Carteira")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_UserWallet",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Chave Primária da Transação"),
                    OriginWalletId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Chave Estrangeira para a Carteira de Origem"),
                    DestineWalletId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Chave Estrangeira para a Carteira de Destino"),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false, comment: "Valor da Transação"),
                    TransactionDate = table.Column<DateTime>(type: "timestamp", nullable: false, comment: "Data da Transação")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Wallet_DestineWalletId",
                        column: x => x.DestineWalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                    table.ForeignKey(
                        name: "FK_Transaction_Wallet_OriginWalletId",
                        column: x => x.OriginWalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestineWalletId",
                table: "Transaction",
                column: "DestineWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OriginWalletId",
                table: "Transaction",
                column: "OriginWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
