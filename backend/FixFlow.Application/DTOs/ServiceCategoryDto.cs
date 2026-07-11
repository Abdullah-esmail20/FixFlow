using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Application.DTOs;

public class ServiceCategoryDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
