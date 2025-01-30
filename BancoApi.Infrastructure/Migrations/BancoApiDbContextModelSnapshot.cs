﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BancoApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BancoApi.Infrastructure.Migrations
{
    [DbContext(typeof(BancoApiDbContext))]
    partial class BancoApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BancoApi.Domain.Entities.TransactionWallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("TransactionId")
                        .HasColumnOrder(1)
                        .HasComment("Chave Primária da Transação");

                    b.Property<Guid>("DestinationWalletId")
                        .HasColumnType("uuid")
                        .HasColumnName("DestineWalletId")
                        .HasColumnOrder(3)
                        .HasComment("Chave Estrangeira para a Carteira de Destino");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("TransactionOperation")
                        .HasColumnOrder(6)
                        .HasComment("Tipo da Transação: Deposit, Withdraw ou Transference");

                    b.Property<Guid>("OriginWalletId")
                        .HasColumnType("uuid")
                        .HasColumnName("OriginWalletId")
                        .HasColumnOrder(2)
                        .HasComment("Chave Estrangeira para a Carteira de Origem");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("TransactionDate")
                        .HasColumnOrder(5)
                        .HasComment("Data da Transação");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Value")
                        .HasColumnOrder(4)
                        .HasComment("Valor da Transação");

                    b.HasKey("Id")
                        .HasName("PK_Transaction");

                    b.HasIndex("DestinationWalletId");

                    b.HasIndex("OriginWalletId");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("BancoApi.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("UserId")
                        .HasColumnOrder(1)
                        .HasComment("Chave Primária do Usuário");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)")
                        .HasColumnName("Cpf")
                        .HasColumnOrder(4)
                        .HasComment("CPF do Usuário");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("LastName")
                        .HasColumnOrder(3)
                        .HasComment("Sobrenome do Usuário");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Name")
                        .HasColumnOrder(2)
                        .HasComment("Nome do Usuário");

                    b.HasKey("Id")
                        .HasName("PK_User");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("BancoApi.Domain.Entities.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("WalletId")
                        .HasColumnOrder(1)
                        .HasComment("Chave Primária da Carteira");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Balance")
                        .HasColumnOrder(3)
                        .HasComment("Saldo da Carteira");

                    b.Property<List<Guid>>("TransactionsId")
                        .IsRequired()
                        .HasColumnType("uuid[]")
                        .HasColumnName("TransactionsId")
                        .HasColumnOrder(4)
                        .HasComment("Lista de IDs das Transações Associadas à Carteira");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId")
                        .HasColumnOrder(2)
                        .HasComment("Chave Estrangeira para o Usuário");

                    b.HasKey("Id")
                        .HasName("PK_Wallet");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallet", (string)null);
                });

            modelBuilder.Entity("BancoApi.Domain.Entities.TransactionWallet", b =>
                {
                    b.HasOne("BancoApi.Domain.Entities.Wallet", "DestinationWallet")
                        .WithMany()
                        .HasForeignKey("DestinationWalletId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BancoApi.Domain.Entities.Wallet", "OriginWallet")
                        .WithMany()
                        .HasForeignKey("OriginWalletId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DestinationWallet");

                    b.Navigation("OriginWallet");
                });

            modelBuilder.Entity("BancoApi.Domain.Entities.User", b =>
                {
                    b.OwnsOne("BancoApi.Domain.ValueObject.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(1024)
                                .HasColumnType("character varying(1024)")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("BancoApi.Domain.ValueObject.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Password");

                            b1.HasKey("UserId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Password")
                        .IsRequired();
                });

            modelBuilder.Entity("BancoApi.Domain.Entities.Wallet", b =>
                {
                    b.HasOne("BancoApi.Domain.Entities.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("BancoApi.Domain.Entities.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserWallet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BancoApi.Domain.Entities.User", b =>
                {
                    b.Navigation("Wallet")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
