using FixFlow.Application.DTOs;
using FixFlow.Application.Interfaces;

namespace FixFlow.Application.Services;

public class ServiceCategoryService : IServiceCategoryService
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository;

    public ServiceCategoryService(IServiceCategoryRepository serviceCategoryRepository)
    {
        _serviceCategoryRepository = serviceCategoryRepository;
    }

    public async Task<List<ServiceCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _serviceCategoryRepository.GetAllAsync(cancellationToken);

        return categories
            .Select(category => new ServiceCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .ToList();
    }
}