using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlteraColumnTypeDeTransactionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transaction",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Data da Transação",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldComment: "Data da Transação");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "Transaction",
                type: "timestamp",
                nullable: false,
                comment: "Data da Transação",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldComment: "Data da Transação");
        }
    }
}
