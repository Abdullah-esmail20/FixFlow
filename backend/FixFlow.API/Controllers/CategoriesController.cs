using FixFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FixFlow.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IServiceCategoryService _serviceCategoryService;

    public CategoriesController(IServiceCategoryService serviceCategoryService)
    {
        _serviceCategoryService = serviceCategoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _serviceCategoryService.GetAllAsync(cancellationToken);

        return Ok(categories);
    }
}
//يرجع كل أنواع الخدمات.