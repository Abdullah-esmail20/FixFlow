using System.ComponentModel.DataAnnotations;
using FixFlow.Domain.Enums;

namespace FixFlow.Application.DTOs;

public class MaintenanceRequestQueryDto
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public RequestStatus? Status { get; set; }

    public RequestPriority? Priority { get; set; }

    public Guid? ServiceCategoryId { get; set; }
}