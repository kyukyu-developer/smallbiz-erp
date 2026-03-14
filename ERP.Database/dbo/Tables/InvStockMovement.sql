CREATE TABLE [dbo].[InvStockMovement] (
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
    [Active]        BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]     DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     NVARCHAR (50)   NULL,
    [UpdatedAt]     DATETIME2 (7)   NULL,
    [UpdatedBy]     NVARCHAR (50)   NULL,
    [LastAction]    NVARCHAR (50)   NULL,
    CONSTRAINT [PK_InvStockMovement] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvStockMovement_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_InvStockMovement_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProdBatch] ([Id]),
    CONSTRAINT [FK_InvStockMovement_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_InvStockMovement_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProdSerial] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockMovement_BatchId]
    ON [dbo].[InvStockMovement]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockMovement_ProductId]
    ON [dbo].[InvStockMovement]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockMovement_SerialId]
    ON [dbo].[InvStockMovement]([SerialId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvStockMovement_WarehouseId]
    ON [dbo].[InvStockMovement]([WarehouseId] ASC);

