using BancoApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Mapping;
public class TransactionMapping : IEntityTypeConfiguration<TransactionWallet>
{
    public void Configure(EntityTypeBuilder<TransactionWallet> builder)
    {
        builder.ToTable("Transaction");

        builder.HasKey(t => t.Id)
            .HasName("PK_Transaction");

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("TransactionId")
            .HasColumnOrder(1)
            .HasComment("Chave Primária da Transação");

        builder.Property(t => t.OriginWalletId)
            .HasColumnName("OriginWalletId")
            .HasColumnOrder(2)
            .HasComment("Chave Estrangeira para a Carteira de Origem");

        builder.Property(t => t.DestinationWalletId)
            .HasColumnName("DestineWalletId")
            .HasColumnOrder(3)
            .HasComment("Chave Estrangeira para a Carteira de Destino");

        builder.Property(t => t.Value)
            .HasColumnName("Value")
            .HasColumnOrder(4)
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasComment("Valor da Transação");

        builder.Property(t => t.TransactionDate)
            .HasColumnName("TransactionDate")
            .HasColumnOrder(5)
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasComment("Data da Transação");

        builder.Property(t => t.Operation)
            .HasColumnName("TransactionOperation")
            .HasColumnOrder(6)
            .HasConversion<string>()
            .IsRequired()
            .HasComment("Tipo da Transação: Deposit, Withdraw ou Transference");


        builder.HasOne(t => t.OriginWallet)
            .WithMany()
            .HasForeignKey(t => t.OriginWalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
