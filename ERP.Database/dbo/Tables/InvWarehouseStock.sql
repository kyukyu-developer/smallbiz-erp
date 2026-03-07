CREATE TABLE [dbo].[InvWarehouseStock] (
    [Id]                VARCHAR (50)    NOT NULL,
    [WarehouseId]       VARCHAR (50)    NOT NULL,
    [ProductId]         VARCHAR (50)    NOT NULL,
    [AvailableQuantity] DECIMAL (18, 2) NOT NULL,
    [ReservedQuantity]  DECIMAL (18, 2) NOT NULL,
    [Active]            BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]         DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]         DATETIME2 (7)   NULL,
    [CreatedBy]         NVARCHAR (50)   NULL,
    [UpdatedBy]         NVARCHAR (50)   NULL,
    [LastAction]        NVARCHAR (50)   NULL,
    CONSTRAINT [PK_InvWarehouseStock] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InvWarehouseStock_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id]),
    CONSTRAINT [FK_InvWarehouseStock_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouseStock_ProductId]
    ON [dbo].[InvWarehouseStock]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InvWarehouseStock_WarehouseId]
    ON [dbo].[InvWarehouseStock]([WarehouseId] ASC);

