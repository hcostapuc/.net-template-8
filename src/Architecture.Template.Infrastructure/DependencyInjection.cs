﻿using Ardalis.GuardClauses;
using Domain.Interfaces.Repository;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ArchitectureTemplate"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Guard.Against.NullOrEmpty(connectionString, message: "Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IWashOrderRepository, WashOrderRepository>();

        return services;
    }
}
