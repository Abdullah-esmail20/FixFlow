using System;
using System.Collections.Generic;
using System.Text;

using FixFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FixFlow.Infrastructure.Configurations;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
    {
        builder.ToTable("MaintenanceRequests");

        builder.HasKey(request => request.Id);

        builder.Property(request => request.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(request => request.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(request => request.CustomerId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(request => request.TechnicianId)
            .HasMaxLength(450);

        builder.Property(request => request.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(request => request.Priority)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(request => request.Location)
            .HasMaxLength(300);

        builder.Property(request => request.CancellationReason)
            .HasMaxLength(500);

        builder.Property(request => request.CreatedAt)
            .IsRequired();

        builder.Property(request => request.UpdatedAt);

        builder.Property(request => request.PreferredDate);

        builder.Property(request => request.AssignedAt);

        builder.Property(request => request.StartedAt);

        builder.Property(request => request.CompletedAt);

        builder.HasOne<ServiceCategory>()
            .WithMany()
            .HasForeignKey(request => request.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(request => request.CustomerId);

        builder.HasIndex(request => request.TechnicianId);

        builder.HasIndex(request => request.Status);
    }
}