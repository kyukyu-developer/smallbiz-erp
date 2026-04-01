CREATE TABLE [dbo].[PurchGoodsReceive] (
    [Id]              VARCHAR (50)  NOT NULL,
    [ReceiveNumber]   NVARCHAR (30) NOT NULL,
    [ReceiveDate]     DATETIME2 (7) NOT NULL,
    [PurchaseOrderId] VARCHAR (50)  NULL,
    [SupplierId]      VARCHAR (50)  NOT NULL,
    [WarehouseId]     VARCHAR (50)  NOT NULL,
    [Status]          INT           NOT NULL DEFAULT 0,
    [Notes]           NVARCHAR (MAX) NULL,
    [Active]          BIT           DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50) NULL,
    [UpdatedAt]       DATETIME2 (7) NULL,
    [UpdatedBy]       NVARCHAR (50) NULL,
    [LastAction]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_PurchGoodsReceive] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchGoodsReceive_PurchOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchOrder] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceive_PurchSupplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[PurchSupplier] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceive_InvWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[InvWarehouse] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_PurchGoodsReceive_ReceiveNumber]
    ON [dbo].[PurchGoodsReceive]([ReceiveNumber] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceive_PurchaseOrderId]
    ON [dbo].[PurchGoodsReceive]([PurchaseOrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceive_SupplierId]
    ON [dbo].[PurchGoodsReceive]([SupplierId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceive_WarehouseId]
    ON [dbo].[PurchGoodsReceive]([WarehouseId] ASC);
