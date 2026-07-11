using System;
using System.Collections.Generic;
using System.Text;

using FixFlow.Domain.Entities;

namespace FixFlow.Application.Interfaces;

public interface IMaintenanceRequestRepository
{
    Task<MaintenanceRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<MaintenanceRequest>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);

    Task<List<MaintenanceRequest>> GetByTechnicianIdAsync(string technicianId, CancellationToken cancellationToken = default);

    Task AddAsync(MaintenanceRequest request, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
