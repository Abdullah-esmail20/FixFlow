using FixFlow.Application.Common;
using FixFlow.Application.DTOs;
using FixFlow.Application.Interfaces;
using FixFlow.Domain.Entities;

namespace FixFlow.Application.Services;

public class MaintenanceRequestService : IMaintenanceRequestService
{
    private readonly IMaintenanceRequestRepository _maintenanceRequestRepository;
    private readonly IServiceCategoryRepository _serviceCategoryRepository;

    public MaintenanceRequestService(
        IMaintenanceRequestRepository maintenanceRequestRepository,
        IServiceCategoryRepository serviceCategoryRepository)
    {
        _maintenanceRequestRepository = maintenanceRequestRepository;
        _serviceCategoryRepository = serviceCategoryRepository;
    }

    public async Task<Result<Guid>> CreateAsync(
        CreateMaintenanceRequestDto dto,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return Result<Guid>.Failure("Customer id is required.");

        var category = await _serviceCategoryRepository.GetByIdAsync(
            dto.ServiceCategoryId,
            cancellationToken);

        if (category is null)
            return Result<Guid>.NotFound("Service category not found.");

        var request = new MaintenanceRequest(
            dto.Title,
            dto.Description,
            customerId,
            dto.ServiceCategoryId,
            dto.Location,
            dto.PreferredDate);

        await _maintenanceRequestRepository.AddAsync(request, cancellationToken);
        await _maintenanceRequestRepository.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(request.Id);
    }

    public async Task<Result<List<MaintenanceRequestDto>>> GetByCustomerIdAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return Result<List<MaintenanceRequestDto>>.Failure("Customer id is required.");

        var requests = await _maintenanceRequestRepository.GetByCustomerIdAsync(
            customerId,
            cancellationToken);

        var result = requests
            .Select(request => new MaintenanceRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CustomerId = request.CustomerId,
                TechnicianId = request.TechnicianId,
                ServiceCategoryId = request.ServiceCategoryId,
                Status = request.Status.ToString(),
                Priority = request.Priority.ToString(),
                Location = request.Location,
                PreferredDate = request.PreferredDate,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt
            })
            .ToList();

        return Result<List<MaintenanceRequestDto>>.Success(result);
    }

    public async Task<Result> AssignTechnicianAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(technicianId))
            return Result.Failure("Technician id is required.");

        var request = await _maintenanceRequestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
            return Result.NotFound("Maintenance request not found.");

        try
        {
            request.AssignTechnician(technicianId);

            await _maintenanceRequestRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> AcceptAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(technicianId))
            return Result.Failure("Technician id is required.");

        var request = await _maintenanceRequestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
            return Result.NotFound("Maintenance request not found.");

        if (request.TechnicianId != technicianId)
            return Result.Forbidden("This request is not assigned to this technician.");

        try
        {
            request.Accept();

            await _maintenanceRequestRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Conflict(ex.Message);
        }
    }

    public async Task<Result> StartWorkAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(technicianId))
            return Result.Failure("Technician id is required.");

        var request = await _maintenanceRequestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
            return Result.NotFound("Maintenance request not found.");

        if (request.TechnicianId != technicianId)
            return Result.Forbidden("This request is not assigned to this technician.");

        try
        {
            request.StartWork();

            await _maintenanceRequestRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Conflict(ex.Message);
        }
    }

    public async Task<Result> CompleteAsync(
        Guid requestId,
        string technicianId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(technicianId))
            return Result.Failure("Technician id is required.");

        var request = await _maintenanceRequestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
            return Result.NotFound("Maintenance request not found.");

        if (request.TechnicianId != technicianId)
            return Result.Forbidden("This request is not assigned to this technician.");

        try
        {
            request.Complete();

            await _maintenanceRequestRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Conflict(ex.Message);
        }
    }

    public async Task<Result> ConfirmByCustomerAsync(
        Guid requestId,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            return Result.Failure("Customer id is required.");

        var request = await _maintenanceRequestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
            return Result.NotFound("Maintenance request not found.");

        if (request.CustomerId != customerId)
            return Result.Forbidden("This request does not belong to this customer.");

        try
        {
            request.ConfirmByCustomer();

            await _maintenanceRequestRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Conflict(ex.Message);
        }
    }

    public async Task<Result<List<MaintenanceRequestDto>>> GetByTechnicianIdAsync(
        string technicianId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(technicianId))
            return Result<List<MaintenanceRequestDto>>.Failure("Technician id is required.");

        var requests = await _maintenanceRequestRepository.GetByTechnicianIdAsync(
            technicianId,
            cancellationToken);

        var result = requests
            .Select(request => new MaintenanceRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CustomerId = request.CustomerId,
                TechnicianId = request.TechnicianId,
                ServiceCategoryId = request.ServiceCategoryId,
                Status = request.Status.ToString(),
                Priority = request.Priority.ToString(),
                Location = request.Location,
                PreferredDate = request.PreferredDate,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt
            })
            .ToList();

        return Result<List<MaintenanceRequestDto>>.Success(result);
    }

    public async Task<Result<MaintenanceRequestDto>> GetByIdAsync(
        Guid requestId,
        string? customerId,
        string? technicianId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(customerId) && string.IsNullOrWhiteSpace(technicianId))
            return Result<MaintenanceRequestDto>.Failure("Customer id or technician id is required.");

        var request = await _maintenanceRequestRepository.GetByIdAsync(
            requestId,
            cancellationToken);

        if (request is null)
            return Result<MaintenanceRequestDto>.NotFound("Maintenance request not found.");

        var isCustomerOwner = !string.IsNullOrWhiteSpace(customerId)
            && request.CustomerId == customerId;

        var isAssignedTechnician = !string.IsNullOrWhiteSpace(technicianId)
            && request.TechnicianId == technicianId;

        if (!isCustomerOwner && !isAssignedTechnician)
            return Result<MaintenanceRequestDto>.Forbidden("You are not allowed to view this request.");

        var result = new MaintenanceRequestDto
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            CustomerId = request.CustomerId,
            TechnicianId = request.TechnicianId,
            ServiceCategoryId = request.ServiceCategoryId,
            Status = request.Status.ToString(),
            Priority = request.Priority.ToString(),
            Location = request.Location,
            PreferredDate = request.PreferredDate,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt
        };

        return Result<MaintenanceRequestDto>.Success(result);
    }

    public async Task<Result<PagedResult<MaintenanceRequestDto>>> GetPagedAsync(
     MaintenanceRequestQueryDto query,
     string? customerId = null,
     string? technicianId = null)
    {
        var (items, totalCount) = await _maintenanceRequestRepository.GetPagedAsync(
     query.PageNumber,
     query.PageSize,
     query.Status,
     query.Priority,
     query.ServiceCategoryId,
     customerId,
     technicianId,
     query.Search,
     query.SortBy,
     query.SortDirection);

        var result = new PagedResult<MaintenanceRequestDto>
        {
            Items = items.Select(request => new MaintenanceRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CustomerId = request.CustomerId,
                TechnicianId = request.TechnicianId,
                ServiceCategoryId = request.ServiceCategoryId,


                Status = request.Status.ToString(),
                Priority = request.Priority.ToString(),

                Location = request.Location,
                PreferredDate = request.PreferredDate,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt
            }).ToList(),

            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<MaintenanceRequestDto>>.Success(result);
    }
}