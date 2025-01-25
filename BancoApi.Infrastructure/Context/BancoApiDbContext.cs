using BancoApi.Domain.Entities;
using BancoApi.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Context;
public class BancoApiDbContext : DbContext
{
    public BancoApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BancoApiDbContext).Assembly);
        base.OnModelCreating(modelBuilder);


        //seeds
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var wallet1Id = Guid.NewGuid();
        var wallet2Id = Guid.NewGuid();

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = user1Id,
                Name = "Aldruin",
                LastName = "Souza",
                Cpf = "05198084956",
                Email = new Email("aldruinsouza@outlook.com"),
                Password = new Password("MeContrata123")
            },
            new User
            {
                Id = user2Id,
                Name = "Roberta",
                LastName = "Souza",
                Cpf = "12345678910",
                Email = new Email("roberta@exemplo.com"),
                Password = new Password("roberta123")
            }
        );

        modelBuilder.Entity<Wallet>().HasData(
            new Wallet
            {
                Id = wallet1Id,
                UserId = user1Id,
                Balance = 1000.00m
            },
            new Wallet
            {
                Id = wallet2Id,
                UserId = user2Id,
                Balance = 500.00m
            }
        );

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                Id = Guid.NewGuid(),
                OriginWalletId = wallet1Id,
                DestineWalletId = wallet2Id,
                Value = 200.00m,
                TransactionDate = DateTime.UtcNow
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                OriginWalletId = wallet2Id,
                DestineWalletId = wallet1Id,
                Value = 50.00m,
                TransactionDate = DateTime.UtcNow
            }
        );
    }
}
