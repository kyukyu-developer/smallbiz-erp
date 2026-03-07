CREATE TABLE [dbo].[SalesInvoiceItem] (
    [Id]              VARCHAR (50)    NOT NULL,
    [SaleId]          VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [UnitId]          VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [UnitPrice]       DECIMAL (18, 2) NOT NULL,
    [DiscountPercent] DECIMAL (18, 2) NULL,
    [DiscountAmount]  DECIMAL (18, 2) NULL,
    [TaxPercent]      DECIMAL (18, 2) NULL,
    [TaxAmount]       DECIMAL (18, 2) NULL,
    [TotalAmount]     DECIMAL (18, 2) NOT NULL,
    [Notes]           NVARCHAR (MAX)  NULL,
    [BatchId]         VARCHAR (50)    NULL,
    [SerialId]        VARCHAR (50)    NULL,
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesInvoiceItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesInvoiceItem_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProdBatch] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProdSerial] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id]),
    CONSTRAINT [FK_SalesInvoiceItem_SalesInvoice] FOREIGN KEY ([SaleId]) REFERENCES [dbo].[SalesInvoice] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoiceItem_BatchId]
    ON [dbo].[SalesInvoiceItem]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoiceItem_ProductId]
    ON [dbo].[SalesInvoiceItem]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoiceItem_SaleId]
    ON [dbo].[SalesInvoiceItem]([SaleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoiceItem_SerialId]
    ON [dbo].[SalesInvoiceItem]([SerialId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesInvoiceItem_UnitId]
    ON [dbo].[SalesInvoiceItem]([UnitId] ASC);

