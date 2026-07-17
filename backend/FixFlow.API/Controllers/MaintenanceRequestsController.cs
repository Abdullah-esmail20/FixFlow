using FixFlow.API.Responses;
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
     ApiResponse<Guid>.Ok(
         result.Value,
         "Maintenance request created successfully."));

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

        return Ok(ApiResponse<object>.Ok(
            result.Value!,
            "Customer maintenance requests retrieved successfully."));
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

        return Ok(ApiResponse<object>.Ok(
            result.Value!,
            "Assigned maintenance requests retrieved successfully."));
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

        return Ok(ApiResponse<object>.Ok(
            result.Value!,
            "Maintenance request retrieved successfully."));
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

        return Ok(ApiResponse.Ok("Technician assigned successfully."));
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

        return Ok(ApiResponse.Ok("Request accepted successfully."));
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

        return Ok(ApiResponse.Ok("Work started successfully."));
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

        return Ok(ApiResponse.Ok("Work completed successfully."));
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

        return Ok(ApiResponse.Ok("Request confirmed by customer successfully."));
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
    /// <summary>
    /// أضف endpoint جديد للأدمن:
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPagedForAdmin([FromQuery] MaintenanceRequestQueryDto query)
    {
        var result = await _maintenanceRequestService.GetPagedAsync(query);

        if (!result.IsSuccess)
        {
            return HandleFailure(result);
        }

        return Ok(ApiResponse<object>.Ok(
            result.Value!,
            "Maintenance requests retrieved successfully."));
    }
    /// <summary>
    /// وأضف endpoint للعميل مع pagination:
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>

    [HttpGet("my-requests/paged")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMyRequestsPaged([FromQuery] MaintenanceRequestQueryDto query)
    {
        var customerId = GetCurrentUserId();

        var result = await _maintenanceRequestService.GetPagedAsync(
            query,
            customerId: customerId);

        if (!result.IsSuccess)
        {
            return HandleFailure(result);
        }

        return Ok(ApiResponse<object>.Ok(
            result.Value!,
            "Customer paged maintenance requests retrieved successfully."));
    }
    /// <summary>
    /// //وأضف endpoint للفني:
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>

    [HttpGet("my-assigned/paged")]
    [Authorize(Roles = "Technician")]
    public async Task<IActionResult> GetMyAssignedRequestsPaged([FromQuery] MaintenanceRequestQueryDto query)
    {
        var technicianId = GetCurrentUserId();

        var result = await _maintenanceRequestService.GetPagedAsync(
            query,
            technicianId: technicianId);

        if (!result.IsSuccess)
        {
            return HandleFailure(result);
        }

        return Ok(ApiResponse<object>.Ok(
            result.Value!,
            "Technician paged maintenance requests retrieved successfully."));
    }




}
 
