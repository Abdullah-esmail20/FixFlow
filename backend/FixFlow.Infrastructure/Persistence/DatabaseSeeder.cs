using System;
using System.Collections.Generic;
using System.Text;
using FixFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixFlow.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.ServiceCategories.AnyAsync())
            return;

        var categories = new List<ServiceCategory>
        {
            new ServiceCategory("Electricity", "Electrical problems such as lights, sockets, and breakers."),
            new ServiceCategory("Plumbing", "Water leaks, pipes, sinks, and bathroom issues."),
            new ServiceCategory("Air Conditioning", "AC cooling, heating, and maintenance issues."),
            new ServiceCategory("Internet", "Internet, network, and router problems."),
            new ServiceCategory("General Maintenance", "General repair and maintenance requests.")
        };

        await context.ServiceCategories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
}