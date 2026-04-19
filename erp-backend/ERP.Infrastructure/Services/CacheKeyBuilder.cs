namespace ERP.Infrastructure.Services;

using ERP.Domain.Interfaces;

public class CacheKeyBuilder : ICacheKeyBuilder
{
    public string Brand_All => Build("brand", "all");
    public string Brand_ById(int id) => Build("brand", id.ToString());

    public string Category_All => Build("category", "all");
    public string Category_ById(int id) => Build("category", id.ToString());

    public string Unit_All => Build("unit", "all");
    public string Unit_ById(int id) => Build("unit", id.ToString());

    public string Warehouse_All => Build("warehouse", "all");
    public string Warehouse_ById(int id) => Build("warehouse", id.ToString());

    public string ProductGroup_All => Build("productgroup", "all");
    public string ProductGroup_ById(int id) => Build("productgroup", id.ToString());

    public string Product_All => Build("product", "all");
    public string Product_ById(int id) => Build("product", id.ToString());
    public string Product_ByCode(string code) => Build("product", code);

    public string Build(string entity, string id) => $"{entity}:{id}";
}
