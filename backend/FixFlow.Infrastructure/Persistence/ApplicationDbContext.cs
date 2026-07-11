using System;
using System.Collections.Generic;
using System.Text;
using FixFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FixFlow.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();

    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
