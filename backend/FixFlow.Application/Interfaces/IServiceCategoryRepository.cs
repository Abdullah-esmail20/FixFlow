using FixFlow.Domain.Entities;

namespace FixFlow.Application.Interfaces;

public interface IServiceCategoryRepository
{
    Task<List<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ServiceCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}