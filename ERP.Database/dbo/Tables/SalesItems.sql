CREATE TABLE [dbo].[SalesItems] (
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
    [Active]          BIT             CONSTRAINT [DF_SalesItems_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   CONSTRAINT [DF_SalesItems_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_SalesItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SalesItems_ProductBatches] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProductBatches] ([Id]),
    CONSTRAINT [FK_SalesItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_SalesItems_ProductSerials] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProductSerials] ([Id]),
    CONSTRAINT [FK_SalesItems_Sales] FOREIGN KEY ([SaleId]) REFERENCES [dbo].[Sales] ([Id]),
    CONSTRAINT [FK_SalesItems_Units] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[Units] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_SalesItems_BatchId]
    ON [dbo].[SalesItems]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesItems_ProductId]
    ON [dbo].[SalesItems]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesItems_SaleId]
    ON [dbo].[SalesItems]([SaleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesItems_SerialId]
    ON [dbo].[SalesItems]([SerialId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SalesItems_UnitId]
    ON [dbo].[SalesItems]([UnitId] ASC);

