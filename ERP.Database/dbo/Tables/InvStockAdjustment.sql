CREATE TABLE [dbo].[InvStockAdjustment] (
    [Id]                 VARCHAR (50)    NOT NULL,
    [AdjustmentNo]       VARCHAR (50)    NOT NULL,
    [WarehouseId]        VARCHAR (50)    NOT NULL,
    [ProductId]          VARCHAR (50)    NOT NULL,
    [AdjustmentQuantity] DECIMAL (18, 2) NOT NULL,
    [Reason]             NVARCHAR (MAX)  NOT NULL,
    [AdjustmentDate]     DATETIME2 (7)   NOT NULL,
    [Active]             BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]          DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          NVARCHAR (50)   NULL,
    [UpdatedAt]          DATETIME2 (7)   NULL,
    [UpdatedBy]          NVARCHAR (50)   NULL,
    [LastAction]         NVARCHAR (50)   NULL,
    CONSTRAINT [PK_InvStockAdjustment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvStockAdjustment_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_InvStockAdjustment_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockAdjustment_ProductId]
    ON [dbo].[InvStockAdjustment]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockAdjustment_WarehouseId]
    ON [dbo].[InvStockAdjustment]([WarehouseId] ASC);

