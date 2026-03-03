CREATE TABLE [dbo].[StockAdjustments] (
    [Id]                 VARCHAR (50)    NOT NULL,
    [AdjustmentNo]       VARCHAR (50)    NOT NULL,
    [WarehouseId]        VARCHAR (50)    NOT NULL,
    [ProductId]          VARCHAR (50)    NOT NULL,
    [AdjustmentQuantity] DECIMAL (18, 2) NOT NULL,
    [Reason]             NVARCHAR (MAX)  NOT NULL,
    [AdjustmentDate]     DATETIME2 (7)   NOT NULL,
    [Active]             BIT             CONSTRAINT [DF_StockAdjustments_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]          DATETIME2 (7)   CONSTRAINT [DF_StockAdjustments_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          NVARCHAR (50)   NULL,
    [UpdatedAt]          DATETIME2 (7)   NULL,
    [UpdatedBy]          NVARCHAR (50)   NULL,
    [LastAction]         NVARCHAR (50)   NULL,
    CONSTRAINT [PK_StockAdjustments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockAdjustments_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_StockAdjustments_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_StockAdjustments_ProductId]
    ON [dbo].[StockAdjustments]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StockAdjustments_WarehouseId]
    ON [dbo].[StockAdjustments]([WarehouseId] ASC);

