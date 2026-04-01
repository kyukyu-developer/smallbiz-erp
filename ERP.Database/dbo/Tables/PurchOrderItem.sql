CREATE TABLE [dbo].[PurchOrderItem] (
    [Id]               VARCHAR (50)    NOT NULL,
    [PurchaseOrderId]  VARCHAR (50)    NOT NULL,
    [ProductId]        VARCHAR (50)    NOT NULL,
    [UnitId]           VARCHAR (50)    NOT NULL,
    [Quantity]         DECIMAL (18, 4) NOT NULL,
    [ReceivedQuantity] DECIMAL (18, 4) NOT NULL DEFAULT 0,
    [UnitCost]         DECIMAL (18, 2) NOT NULL,
    [DiscountPercent]  DECIMAL (18, 2) NULL DEFAULT 0,
    [DiscountAmount]   DECIMAL (18, 2) NULL DEFAULT 0,
    [TaxPercent]       DECIMAL (18, 2) NULL DEFAULT 0,
    [TaxAmount]        DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalAmount]      DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [Notes]            NVARCHAR (MAX)  NULL,
    [Active]           BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]        DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        NVARCHAR (50)   NULL,
    [UpdatedAt]        DATETIME2 (7)   NULL,
    [UpdatedBy]        NVARCHAR (50)   NULL,
    [LastAction]       NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchOrderItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchOrderItem_PurchOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchOrder] ([Id]),
    CONSTRAINT [FK_PurchOrderItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_PurchOrderItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PurchOrderItem_PurchaseOrderId]
    ON [dbo].[PurchOrderItem]([PurchaseOrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchOrderItem_ProductId]
    ON [dbo].[PurchOrderItem]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchOrderItem_UnitId]
    ON [dbo].[PurchOrderItem]([UnitId] ASC);
