using BancoApi.Domain.Entities;
using BancoApi.Domain.Enums;
using BancoApi.Domain.Repositories;
using BancoApi.Domain.ValueObject;
using BancoApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Data;
public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider, BancoApiDbContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    Name = "João",
                    LastName = "Silva",
                    Cpf = "12345678901",
                    Email = new Email { Value = "joao.silva@example.com" },
                    Password = new Password { Value = "Senha123" }
                },
                new User
                {
                    Name = "Maria",
                    LastName = "Oliveira",
                    Cpf = "23456789012",
                    Email = new Email { Value = "maria.oliveira@example.com" },
                    Password = new Password { Value = "Senha123" }
                },
                new User
                {
                    Name = "Carlos",
                    LastName = "Santos",
                    Cpf = "34567890123",
                    Email = new Email { Value = "carlos.santos@example.com" },
                    Password = new Password { Value = "Senha123" }
                }
            };

            foreach (var user in users)
            {
                user.SetPassword();
                context.Users.Add(user);

                
                var wallet1 = new Wallet
                {
                    User = user,
                    Balance = 1000.00m 
                };

                var wallet2 = new Wallet
                {
                    User = user,
                    Balance = 500.00m
                };

                var wallet3 = new Wallet
                {
                    User = user,
                    Balance = 200.00m
                };

                context.Wallets.AddRange(wallet1, wallet2, wallet3);
                user.Wallet = wallet1;
            }

            context.SaveChanges(); 


            var wallets = context.Wallets.ToList();

            var transactionTypes = Enum.GetValues(typeof(TransactionOperation)).Cast<TransactionOperation>().ToList();

            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var originWallet = wallets[random.Next(wallets.Count)];
                var destinationWallet = wallets[random.Next(wallets.Count)];
                while (originWallet.Id == destinationWallet.Id)
                {
                    destinationWallet = wallets[random.Next(wallets.Count)];
                }

                var transaction = new TransactionWallet
                {
                    OriginWallet = originWallet,
                    DestinationWallet = destinationWallet,
                    Value = (decimal)(random.NextDouble() * 500),
                    TransactionDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)), 
                    Operation = transactionTypes[random.Next(transactionTypes.Count)]
                };

                context.Transactions.Add(transaction);
            }

            context.SaveChanges();
        }
    }
}