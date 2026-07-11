using System;
using System.Collections.Generic;
using System.Text;
using FixFlow.Domain.Common;
using FixFlow.Domain.Enums;

namespace FixFlow.Domain.Entities;

public class MaintenanceRequest : BaseEntity
{
    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string CustomerId { get; private set; } = string.Empty;

    public string? TechnicianId { get; private set; }

    public Guid ServiceCategoryId { get; private set; }

    public RequestStatus Status { get; private set; } = RequestStatus.Created;

    public RequestPriority Priority { get; private set; } = RequestPriority.Medium;

    public string? Location { get; private set; }

    public DateTime? PreferredDate { get; private set; }

    public string? CancellationReason { get; private set; }

    public DateTime? AssignedAt { get; private set; }

    public DateTime? StartedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    private MaintenanceRequest()
    {
    }

    public MaintenanceRequest(
        string title,
        string description,
        string customerId,
        Guid serviceCategoryId,
        string? location,
        DateTime? preferredDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.");

        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("Customer id is required.");

        Title = title.Trim();
        Description = description.Trim();
        CustomerId = customerId;
        ServiceCategoryId = serviceCategoryId;
        Location = location?.Trim();
        PreferredDate = preferredDate;
    }

    public void AssignTechnician(string technicianId)
    {
        if (string.IsNullOrWhiteSpace(technicianId))
            throw new ArgumentException("Technician id is required.");

        if (Status != RequestStatus.Created && Status != RequestStatus.OnHold)
            throw new InvalidOperationException("Only created or on-hold requests can be assigned.");

        TechnicianId = technicianId;
        Status = RequestStatus.Assigned;
        AssignedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void Accept()
    {
        if (Status != RequestStatus.Assigned)
            throw new InvalidOperationException("Only assigned requests can be accepted.");

        Status = RequestStatus.Accepted;
        SetUpdated();
    }

    public void StartWork()
    {
        if (Status != RequestStatus.Accepted)
            throw new InvalidOperationException("Only accepted requests can be started.");

        Status = RequestStatus.InProgress;
        StartedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void Complete()
    {
        if (Status != RequestStatus.InProgress)
            throw new InvalidOperationException("Only in-progress requests can be completed.");

        Status = RequestStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        SetUpdated();
    }

    public void ConfirmByCustomer()
    {
        if (Status != RequestStatus.Completed)
            throw new InvalidOperationException("Only completed requests can be confirmed by customer.");

        Status = RequestStatus.CustomerConfirmed;
        SetUpdated();
    }

    public void Cancel(string reason)
    {
        if (Status == RequestStatus.Completed || Status == RequestStatus.CustomerConfirmed)
            throw new InvalidOperationException("Completed requests cannot be cancelled.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required.");

        Status = RequestStatus.Cancelled;
        CancellationReason = reason.Trim();
        SetUpdated();
    }

    public void ChangePriority(RequestPriority priority)
    {
        Priority = priority;
        SetUpdated();
    }
 
}
