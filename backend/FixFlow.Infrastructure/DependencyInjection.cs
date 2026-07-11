using FixFlow.Application.Interfaces;
using FixFlow.Infrastructure.Persistence;
using FixFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FixFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();
        services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();

        return services;
    }
}