CREATE TABLE [dbo].[WarehouseStocks] (
    [Id]                VARCHAR (50)    NOT NULL,
    [WarehouseId]       VARCHAR (50)    NOT NULL,
    [ProductId]         VARCHAR (50)    NOT NULL,
    [AvailableQuantity] DECIMAL (18, 2) NOT NULL,
    [ReservedQuantity]  DECIMAL (18, 2) NOT NULL,
    [Active]            BIT             CONSTRAINT [DF_WarehouseStocks_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]         DATETIME2 (7)   CONSTRAINT [DF_WarehouseStocks_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]         DATETIME2 (7)   NULL,
    [CreatedBy]         NVARCHAR (50)   NULL,
    [UpdatedBy]         NVARCHAR (50)   NULL,
    [LastAction]        NVARCHAR (50)   NULL,
    CONSTRAINT [PK_WarehouseStocks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WarehouseStocks_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_WarehouseStocks_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_WarehouseStocks_ProductId]
    ON [dbo].[WarehouseStocks]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WarehouseStocks_WarehouseId]
    ON [dbo].[WarehouseStocks]([WarehouseId] ASC);

