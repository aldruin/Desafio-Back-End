using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTransactionOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionOperation",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "Tipo da Transação: Deposit, Withdraw ou Transference")
                .Annotation("Relational:ColumnOrder", 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionOperation",
                table: "Transaction");
        }
    }
}
