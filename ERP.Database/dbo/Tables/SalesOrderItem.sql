CREATE TABLE [dbo].[SalesOrderItem] (
    [Id]                VARCHAR (50)    NOT NULL,
    [SalesOrderId]      VARCHAR (50)    NOT NULL,
    [ProductId]         VARCHAR (50)    NOT NULL,
    [UnitId]            VARCHAR (50)    NOT NULL,
    [Quantity]          DECIMAL (18, 4) NOT NULL,
    [InvoicedQuantity]  DECIMAL (18, 4) NOT NULL DEFAULT 0,
    [UnitPrice]         DECIMAL (18, 2) NOT NULL,
    [DiscountPercent]   DECIMAL (18, 2) NULL DEFAULT 0,
    [DiscountAmount]    DECIMAL (18, 2) NULL DEFAULT 0,
    [TaxPercent]        DECIMAL (18, 2) NULL DEFAULT 0,
    [TaxAmount]         DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalAmount]       DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [Notes]             NVARCHAR (MAX)  NULL,
    [Active]            BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]         DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         NVARCHAR (50)   NULL,
    [UpdatedAt]         DATETIME2 (7)   NULL,
    [UpdatedBy]         NVARCHAR (50)   NULL,
    [LastAction]        NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesOrderItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesOrderItem_SalesOrder] FOREIGN KEY ([SalesOrderId]) REFERENCES [dbo].[SalesOrder] ([Id]),
    CONSTRAINT [FK_SalesOrderItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_SalesOrderItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_SalesOrderItem_SalesOrderId]
    ON [dbo].[SalesOrderItem]([SalesOrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesOrderItem_ProductId]
    ON [dbo].[SalesOrderItem]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesOrderItem_UnitId]
    ON [dbo].[SalesOrderItem]([UnitId] ASC);
