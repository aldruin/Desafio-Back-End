using BancoApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Mapping;
public class WalletMapping : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallet");

        builder.HasKey(w => w.Id)
            .HasName("PK_Wallet");

        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("WalletId")
            .HasColumnOrder(1)
            .HasComment("Chave Primária da Carteira");

        builder.Property(w => w.UserId)
            .HasColumnName("UserId")
            .HasColumnOrder(2)
            .HasComment("Chave Estrangeira para o Usuário");

        builder.Property(w => w.Balance)
            .HasColumnName("Balance")
            .HasColumnOrder(3)
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasComment("Saldo da Carteira");

        builder.HasMany(w => w.Transactions)
            .WithOne(t => t.OriginWallet)
            .HasForeignKey(t => t.OriginWalletId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(w => w.Transactions)
            .WithOne(t => t.DestineWallet)
            .HasForeignKey(t => t.DestineWalletId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
