CREATE TABLE [dbo].[ProdBatch] (
    [Id]              VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [WarehouseId]     VARCHAR (50)    NOT NULL,
    [BatchNo]         VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [ManufactureDate] DATETIME2 (7)   NULL,
    [ExpiryDate]      DATETIME2 (7)   NULL,
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProdBatch] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProdBatch_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_ProdBatch_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdBatch_ProductId]
    ON [dbo].[ProdBatch]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdBatch_WarehouseId]
    ON [dbo].[ProdBatch]([WarehouseId] ASC);

