using System.ComponentModel.DataAnnotations;

namespace FixFlow.Application.DTOs;

public class CreateMaintenanceRequestDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(10)]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid ServiceCategoryId { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public DateTime? PreferredDate { get; set; }
}