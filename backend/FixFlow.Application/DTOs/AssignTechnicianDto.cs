using System.ComponentModel.DataAnnotations;

namespace FixFlow.Application.DTOs;

public class AssignTechnicianDto
{
    [Required]
    public string TechnicianId { get; set; } = string.Empty;
}