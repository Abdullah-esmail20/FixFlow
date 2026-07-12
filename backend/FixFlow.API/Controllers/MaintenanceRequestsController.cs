using FixFlow.Application.Common;
using FixFlow.Application.DTOs;
using FixFlow.Application.Services;
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

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMaintenanceRequestDto dto,
        [FromHeader(Name = "X-Customer-Id")] string customerId,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.CreateAsync(
            dto,
            customerId,
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

    [HttpGet("my-requests")]
    public async Task<IActionResult> GetMyRequests(
        [FromHeader(Name = "X-Customer-Id")] string customerId,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.GetByCustomerIdAsync(
            customerId,
            cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

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

    [HttpPut("{id:guid}/accept")]
    public async Task<IActionResult> Accept(
        Guid id,
        [FromHeader(Name = "X-Technician-Id")] string technicianId,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.AcceptAsync(
            id,
            technicianId,
            cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/start-work")]
    public async Task<IActionResult> StartWork(
        Guid id,
        [FromHeader(Name = "X-Technician-Id")] string technicianId,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.StartWorkAsync(
            id,
            technicianId,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromHeader(Name = "X-Technician-Id")] string technicianId,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.CompleteAsync(
            id,
            technicianId,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpPut("{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmByCustomer(
        Guid id,
        [FromHeader(Name = "X-Customer-Id")] string customerId,
        CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.ConfirmByCustomerAsync(
            id,
            customerId,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
    //يعني الفني يشوف الطلبات المسندة له.

    [HttpGet("my-assigned")]
    public async Task<IActionResult> GetMyAssignedRequests(
    [FromHeader(Name = "X-Technician-Id")] string technicianId,
    CancellationToken cancellationToken)
    {
        var result = await _maintenanceRequestService.GetByTechnicianIdAsync(
            technicianId,
            cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
    //يعني عرض تفاصيل طلب واحد.
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
    Guid id,
    [FromHeader(Name = "X-Customer-Id")] string? customerId,
    [FromHeader(Name = "X-Technician-Id")] string? technicianId,
    CancellationToken cancellationToken)
    {
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