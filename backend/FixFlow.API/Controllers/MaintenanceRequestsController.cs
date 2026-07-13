
using System.Security.Claims;
using FixFlow.Application.Common;
using FixFlow.Application.DTOs;
using FixFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FixFlow.API.Controllers;

[ApiController]
[Route("api/maintenance-requests")]
public class MaintenanceRequestsController : ControllerBase
{
    private readonly IMaintenanceRequestService _maintenanceRequestService;

    public MaintenanceRequestsController(IMaintenanceRequestService maintenanceRequestService)
    {
        _maintenanceRequestService = maintenanceRequestService;
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMaintenanceRequestDto dto,
        CancellationToken cancellationToken)
    {
        var customerId = GetCurrentUserId();

        var result = await _maintenanceRequestService.CreateAsync(
            dto,
            customerId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Created(
            $"api/maintenance-requests/{result.Value}",
            new
            {
                id = result.Value
            });
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("my-requests")]
    public async Task<IActionResult> GetMyRequests(CancellationToken cancellationToken)
    {
        var customerId = GetCurrentUserId();

        var result = await _maintenanceRequestService.GetByCustomerIdAsync(
            customerId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = "Technician")]
    [HttpGet("my-assigned")]
    public async Task<IActionResult> GetMyAssignedRequests(CancellationToken cancellationToken)
    {
        var technicianId = GetCurrentUserId();

        var result = await _maintenanceRequestService.GetByTechnicianIdAsync(
            technicianId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = "Customer,Technician")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();

        string? customerId = null;
        string? technicianId = null;

        if (User.IsInRole("Customer"))
        {
            customerId = currentUserId;
        }

        if (User.IsInRole("Technician"))
        {
            technicianId = currentUserId;
        }

        var result = await _maintenanceRequestService.GetByIdAsync(
            id,
            customerId,
            technicianId,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/assign-technician")]
    public async Task<IActionResult> AssignTechnician(
        Guid id,
        [FromBody] AssignTechnicianDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.AssignTechnicianAsync(
            id,
            dto.TechnicianId,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "Technician")]
    [HttpPut("{id:guid}/accept")]
    public async Task<IActionResult> Accept(
        Guid id,
        CancellationToken cancellationToken)
    {
        var technicianId = GetCurrentUserId();

        var result = await _maintenanceRequestService.AcceptAsync(
            id,
            technicianId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "Technician")]
    [HttpPut("{id:guid}/start-work")]
    public async Task<IActionResult> StartWork(
        Guid id,
        CancellationToken cancellationToken)
    {
        var technicianId = GetCurrentUserId();

        var result = await _maintenanceRequestService.StartWorkAsync(
            id,
            technicianId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "Technician")]
    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var technicianId = GetCurrentUserId();

        var result = await _maintenanceRequestService.CompleteAsync(
            id,
            technicianId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "Customer")]
    [HttpPut("{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmByCustomer(
        Guid id,
        CancellationToken cancellationToken)
    {
        var customerId = GetCurrentUserId();

        var result = await _maintenanceRequestService.ConfirmByCustomerAsync(
            id,
            customerId ?? string.Empty,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }


    //احترافيًا نرجع 404 للطلب غير الموجود و403 لعدم الصلاحية.

    private IActionResult HandleFailure(Result result)
    {
        var response = new
        {
            error = result.Error
        };

        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(response),
            ErrorType.Forbidden => StatusCode(StatusCodes.Status403Forbidden, response),
            ErrorType.Conflict => Conflict(response),
            _ => BadRequest(response)
        };
    }
}
 
