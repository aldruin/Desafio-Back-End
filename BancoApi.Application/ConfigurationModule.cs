using BancoApi.Application.Handlers;
using BancoApi.Application.Notifications;
using BancoApi.Application.Transactions.Services;
using BancoApi.Application.Users.Services;
using BancoApi.Application.Wallets.Services;
using BancoApi.Domain.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application;
public static class ConfigurationModule
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new List<Notification>());
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<INotificationHandler, NotificationHandler>();
        services.AddScoped<ITransactionService, TransactionService>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        var info = new OpenApiInfo();
        info.Version = "V1";
        info.Title = "API Banco - Desafio Back-End";

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", info);
        //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        Description = "Insira o token JWT desta maneira : Bearer {seu token}",
        //        Name = "Authorization",
        //        BearerFormat = "JWT",
        //        In = ParameterLocation.Header,
        //        Type = SecuritySchemeType.ApiKey,
        //        Scheme = "Bearer"

        //    });
        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        //        {
        //            {
        //                new OpenApiSecurityScheme
        //                {
        //                    Reference = new OpenApiReference
        //                    {
        //                        Type = ReferenceType.SecurityScheme,
        //                        Id = "Bearer"
        //                    },
        //                    Scheme = "oauth2",
        //                    Name = "Bearer",
        //                    In= ParameterLocation.Header,

        //                },
        //                new List<string>()
        //            }
        //        });
        //});

        return services;
    }
}
