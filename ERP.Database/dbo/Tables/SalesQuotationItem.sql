CREATE TABLE [dbo].[SalesQuotationItem] (
    [Id]              VARCHAR (50)    NOT NULL,
    [QuotationId]     VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [UnitId]          VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 4) NOT NULL,
    [UnitPrice]       DECIMAL (18, 2) NOT NULL,
    [DiscountPercent] DECIMAL (18, 2) NULL DEFAULT 0,
    [DiscountAmount]  DECIMAL (18, 2) NULL DEFAULT 0,
    [TaxPercent]      DECIMAL (18, 2) NULL DEFAULT 0,
    [TaxAmount]       DECIMAL (18, 2) NULL DEFAULT 0,
    [TotalAmount]     DECIMAL (18, 2) NOT NULL DEFAULT 0,
    [Notes]           NVARCHAR (MAX)  NULL,
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesQuotationItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesQuotationItem_SalesQuotation] FOREIGN KEY ([QuotationId]) REFERENCES [dbo].[SalesQuotation] ([Id]),
    CONSTRAINT [FK_SalesQuotationItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_SalesQuotationItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_SalesQuotationItem_QuotationId]
    ON [dbo].[SalesQuotationItem]([QuotationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesQuotationItem_ProductId]
    ON [dbo].[SalesQuotationItem]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesQuotationItem_UnitId]
    ON [dbo].[SalesQuotationItem]([UnitId] ASC);
