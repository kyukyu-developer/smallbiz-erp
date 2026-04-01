CREATE TABLE [dbo].[PurchGoodsReceiveItem] (
    [Id]                  VARCHAR (50)    NOT NULL,
    [GoodsReceiveId]      VARCHAR (50)    NOT NULL,
    [PurchaseOrderItemId] VARCHAR (50)    NULL,
    [ProductId]           VARCHAR (50)    NOT NULL,
    [UnitId]              VARCHAR (50)    NOT NULL,
    [Quantity]            DECIMAL (18, 4) NOT NULL,
    [UnitCost]            DECIMAL (18, 2) NOT NULL,
    [BatchId]             VARCHAR (50)    NULL,
    [SerialId]            VARCHAR (50)    NULL,
    [Notes]               NVARCHAR (MAX)  NULL,
    [Active]              BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]           DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]           NVARCHAR (50)   NULL,
    [UpdatedAt]           DATETIME2 (7)   NULL,
    [UpdatedBy]           NVARCHAR (50)   NULL,
    [LastAction]          NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchGoodsReceiveItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchGoodsReceiveItem_PurchGoodsReceive] FOREIGN KEY ([GoodsReceiveId]) REFERENCES [dbo].[PurchGoodsReceive] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceiveItem_PurchOrderItem] FOREIGN KEY ([PurchaseOrderItemId]) REFERENCES [dbo].[PurchOrderItem] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceiveItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceiveItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceiveItem_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProdBatch] ([Id]),
    CONSTRAINT [FK_PurchGoodsReceiveItem_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProdSerial] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceiveItem_GoodsReceiveId]
    ON [dbo].[PurchGoodsReceiveItem]([GoodsReceiveId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceiveItem_PurchaseOrderItemId]
    ON [dbo].[PurchGoodsReceiveItem]([PurchaseOrderItemId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceiveItem_ProductId]
    ON [dbo].[PurchGoodsReceiveItem]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceiveItem_UnitId]
    ON [dbo].[PurchGoodsReceiveItem]([UnitId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceiveItem_BatchId]
    ON [dbo].[PurchGoodsReceiveItem]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchGoodsReceiveItem_SerialId]
    ON [dbo].[PurchGoodsReceiveItem]([SerialId] ASC);
