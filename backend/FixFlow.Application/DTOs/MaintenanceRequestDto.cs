using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Application.DTOs;

public class MaintenanceRequestDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CustomerId { get; set; } = string.Empty;

    public string? TechnicianId { get; set; }

    public Guid ServiceCategoryId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Priority { get; set; } = string.Empty;

    public string? Location { get; set; }

    public DateTime? PreferredDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}