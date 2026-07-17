using FixFlow.Application.Interfaces;
using FixFlow.Domain.Entities;
using FixFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using FixFlow.Infrastructure.Persistence;

using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Infrastructure.Repositories;

public class MaintenanceRequestRepository : IMaintenanceRequestRepository
{
    private readonly ApplicationDbContext _context;

    public MaintenanceRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenanceRequest?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.MaintenanceRequests
            .FirstOrDefaultAsync(request => request.Id == id, cancellationToken);
    }

    public async Task<List<MaintenanceRequest>> GetByCustomerIdAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MaintenanceRequests
            .AsNoTracking()
            .Where(request => request.CustomerId == customerId)
            .OrderByDescending(request => request.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<MaintenanceRequest>> GetByTechnicianIdAsync(
        string technicianId,
        CancellationToken cancellationToken = default)
    {
        return await _context.MaintenanceRequests
            .AsNoTracking()
            .Where(request => request.TechnicianId == technicianId)
            .OrderByDescending(request => request.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        MaintenanceRequest request,
        CancellationToken cancellationToken = default)
    {
        await _context.MaintenanceRequests.AddAsync(request, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<MaintenanceRequest> Items, int TotalCount)> GetPagedAsync(
       int pageNumber,
       int pageSize,
       RequestStatus? status = null,
       RequestPriority? priority = null,
       Guid? serviceCategoryId = null,
       string? customerId = null,
       string? technicianId = null,
       string? search = null, //search
       string? sortBy = null,//Sorting
       string? sortDirection = "desc")//Sorting
    {
        var query = _context.MaintenanceRequests
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(r => r.Priority == priority.Value);
        }

        if (serviceCategoryId.HasValue)
        {
            query = query.Where(r => r.ServiceCategoryId == serviceCategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(customerId))
        {
            query = query.Where(r => r.CustomerId == customerId);
        }

        if (!string.IsNullOrWhiteSpace(technicianId))
        {
            query = query.Where(r => r.TechnicianId == technicianId);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();

            query = query.Where(r =>
                r.Title.Contains(searchTerm) ||
                r.Description.Contains(searchTerm) ||
                (r.Location != null && r.Location.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync();

        var isDescending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLower() switch
        {
            "title" => isDescending
                ? query.OrderByDescending(r => r.Title)
                : query.OrderBy(r => r.Title),

            "status" => isDescending
                ? query.OrderByDescending(r => r.Status)
                : query.OrderBy(r => r.Status),

            "priority" => isDescending
                ? query.OrderByDescending(r => r.Priority)
                : query.OrderBy(r => r.Priority),

            "updatedat" => isDescending
                ? query.OrderByDescending(r => r.UpdatedAt)
                : query.OrderBy(r => r.UpdatedAt),

            "createdat" => isDescending
                ? query.OrderByDescending(r => r.CreatedAt)
                : query.OrderBy(r => r.CreatedAt),

            _ => query.OrderByDescending(r => r.CreatedAt)
        };

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

}
