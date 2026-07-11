using System;
using System.Collections.Generic;
using System.Text;

using FixFlow.Application.DTOs;

namespace FixFlow.Application.Services;

public interface IServiceCategoryService
{
    Task<List<ServiceCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
}