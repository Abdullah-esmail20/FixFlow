using FixFlow.Application.Interfaces;
using FixFlow.Domain.Entities;
using FixFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FixFlow.Infrastructure.Repositories;

public class ServiceCategoryRepository : IServiceCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceCategory?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ServiceCategories
            .AsNoTracking()
            .AnyAsync(category => category.Id == id, cancellationToken);
    }
}