using FixFlow.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FixFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
        services.AddScoped<IServiceCategoryService, ServiceCategoryService>();

        return services;
    }
}