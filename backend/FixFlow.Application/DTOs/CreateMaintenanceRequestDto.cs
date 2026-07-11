using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Application.DTOs;

public class CreateMaintenanceRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid ServiceCategoryId { get; set; }

    public string? Location { get; set; }

    public DateTime? PreferredDate { get; set; }
}