namespace ERP.Domain.Interfaces;

public interface ICacheKeyBuilder
{
    string Brand_All { get; }
    string Brand_ById(int id);

    string Category_All { get; }
    string Category_ById(int id);

    string Unit_All { get; }
    string Unit_ById(int id);

    string Warehouse_All { get; }
    string Warehouse_ById(int id);

    string ProductGroup_All { get; }
    string ProductGroup_ById(int id);

    string Product_All { get; }
    string Product_ById(int id);
    string Product_ByCode(string code);

    string Build(string entity, string id);
}
