using System;
using System.Collections.Generic;
using System.Text;

using FixFlow.Application.Interfaces;
using FixFlow.Domain.Entities;
using FixFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
}
