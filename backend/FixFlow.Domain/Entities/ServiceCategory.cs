using FixFlow.Domain.Common;

namespace FixFlow.Domain.Entities;

public class ServiceCategory : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private ServiceCategory()
    {
    }

    public ServiceCategory(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Service category name is required.");

        Name = name.Trim();
        Description = description?.Trim();
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Service category name is required.");

        Name = name.Trim();
        Description = description?.Trim();
        SetUpdated();
    }
}