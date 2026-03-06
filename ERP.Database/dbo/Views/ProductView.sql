-- Product View: base on Product table
CREATE VIEW [dbo].[ProductView] AS
SELECT
    p.[Id] AS [ProductId],
    p.[Code],
    p.[Name],
    NULL AS [Description],
    (SELECT c.[Name] FROM [dbo].[Categories] c WHERE c.[Id] = p.[CategoryId]) AS [CategoryName],
    (SELECT u.[Name] FROM [dbo].[Units] u WHERE u.[Id] = p.[BaseUnitId]) AS [BaseUnitName],
    NULL AS [Barcode],
    NULL AS [MinimumStock],
    NULL AS [MaximumStock],
    p.[ReorderLevel],
    CAST(p.[HasBatchNumber] AS BIT) AS [IsBatchTracked],
    CAST(p.[HasSerialNumber] AS BIT) AS [IsSerialTracked],
    p.[Active] AS [IsActive],
    p.[CreatedAt] AS [CreatedAt],
    p.[CreatedBy] AS [CreatedBy],
    p.[UpdatedAt] AS [UpdatedAt],
    p.[UpdatedBy] AS [UpdatedBy],
    p.[LastAction] AS [LastAction],
    ISNULL(ws.[TotalAvailable], 0) AS [AvailableStock]
FROM [dbo].[Product] p
LEFT JOIN (
    SELECT [ProductId], SUM([AvailableQuantity]) AS TotalAvailable
    FROM [dbo].[WarehouseStocks]
    WHERE [Active] = 1
    GROUP BY [ProductId]
) ws ON ws.[ProductId] = p.[Id]
WHERE p.[Active] = 1;
GO
