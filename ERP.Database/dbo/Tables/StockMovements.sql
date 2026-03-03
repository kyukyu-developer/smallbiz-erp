CREATE TABLE [dbo].[StockMovements] (
    [Id]            VARCHAR (50)    NOT NULL,
    [ProductId]     VARCHAR (50)    NOT NULL,
    [WarehouseId]   VARCHAR (50)    NOT NULL,
    [MovementType]  VARCHAR (50)    NOT NULL,
    [ReferenceType] INT             NOT NULL,
    [ReferenceId]   VARCHAR (50)    NOT NULL,
    [BaseQuantity]  DECIMAL (18, 2) NOT NULL,
    [BatchId]       VARCHAR (50)    NULL,
    [SerialId]      VARCHAR (50)    NULL,
    [MovementDate]  DATETIME2 (7)   NOT NULL,
    [Notes]         NVARCHAR (MAX)  NULL,
    [Active]        BIT             CONSTRAINT [DF_StockMovements_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]     DATETIME2 (7)   CONSTRAINT [DF_StockMovements_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     NVARCHAR (50)   NULL,
    [UpdatedAt]     DATETIME2 (7)   NULL,
    [UpdatedBy]     NVARCHAR (50)   NULL,
    [LastAction]    NVARCHAR (50)   NULL,
    CONSTRAINT [PK_StockMovements] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockMovements_ProductBatches] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProductBatches] ([Id]),
    CONSTRAINT [FK_StockMovements_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_StockMovements_ProductSerials] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProductSerials] ([Id]),
    CONSTRAINT [FK_StockMovements_Warehouses] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_StockMovements_BatchId]
    ON [dbo].[StockMovements]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StockMovements_ProductId]
    ON [dbo].[StockMovements]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StockMovements_WarehouseId]
    ON [dbo].[StockMovements]([WarehouseId] ASC);

