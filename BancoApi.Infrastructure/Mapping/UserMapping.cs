using BancoApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Mapping;
public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.Id)
            .HasName("PK_User");

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("UserId")
            .HasColumnOrder(1)
            .HasComment("Chave Primária do Usuário");

        builder.Property(u => u.Name)
            .HasColumnName("Name")
            .HasColumnOrder(2)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("Nome do Usuário");

        builder.Property(u => u.LastName)
            .HasColumnName("LastName")
            .HasColumnOrder(3)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("Sobrenome do Usuário");

        builder.Property(u => u.Cpf)
            .HasColumnName("Cpf")
            .HasColumnOrder(4)
            .HasMaxLength(11)
            .IsRequired()
            .HasComment("CPF do Usuário");

        builder.OwnsOne(x => x.Email, p =>
        {
            p.Property(f => f.Value).HasColumnName("Email").IsRequired().HasMaxLength(1024);
        });

        builder.OwnsOne(x => x.Password, p =>
        {
            p.Property(f => f.Value).HasColumnName("Password").IsRequired();
        });

        builder.HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId)
            .HasConstraintName("FK_UserWallet")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
