using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Unit : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
