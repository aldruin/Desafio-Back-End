using BancoApi.Application.Account.Auth.Services;
using BancoApi.Application.Account.Jwt.Services;
using BancoApi.Application.Handlers;
using BancoApi.Application.Notifications;
using BancoApi.Application.Transactions.Services;
using BancoApi.Application.Users.Services;
using BancoApi.Application.Wallets.Services;
using BancoApi.Domain.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSecurity:SecurityKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
            //
            //x.Events = new JwtBearerEvents
            //{
            //    OnTokenValidated = context =>
            //    {
            //        var claims = context.Principal.Claims;
            //        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            //        if (string.IsNullOrEmpty(roleClaim))
            //        {
            //            context.Fail("Role claim is missing.");
            //        }

            //        return Task.CompletedTask;
            //    }
            //};
        });

        var info = new OpenApiInfo();
        info.Version = "V1";
        info.Title = "API Banco - Desafio Back-End";

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", info);
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira : Bearer {seu token}",
                Name = "Authorization",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"

            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "bearer",
                            Name = "Bearer",
                            In= ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
        });

        return services;
    }
}
