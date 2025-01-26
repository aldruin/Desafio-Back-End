﻿using BancoApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure;
public static class ConfigurationModule
{
    public static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(Repository<>));

        return services;
    }
}