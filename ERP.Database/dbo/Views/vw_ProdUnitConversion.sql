CREATE VIEW [dbo].[vw_ProdUnitConversion]
AS
SELECT 
    uc.Id,
    uc.ProductId,
    p.Code AS ProductCode,
    p.Name AS ProductName,
    uc.FromUnitId,
    fu.Name AS FromUnitName,
    fu.Symbol AS FromUnitSymbol,
    uc.ToUnitId,
    tu.Name AS ToUnitName,
    tu.Symbol AS ToUnitSymbol,
    uc.Factor,
    uc.Active,
    uc.CreatedAt,
    uc.CreatedBy,
    uc.UpdatedAt,
    uc.UpdatedBy,
    uc.LastAction
FROM [dbo].[ProdUnitConversion] uc
LEFT JOIN [dbo].[ProdItem] p ON uc.ProductId = p.Id
LEFT JOIN [dbo].[ProdUnit] fu ON uc.FromUnitId = fu.Id
LEFT JOIN [dbo].[ProdUnit] tu ON uc.ToUnitId = tu.Id;