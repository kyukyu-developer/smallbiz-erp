CREATE TABLE [dbo].[PurchaseItems] (
    [Id]              VARCHAR (50)    NOT NULL,
    [PurchaseId]      VARCHAR (50)    NOT NULL,
    [ProductId]       VARCHAR (50)    NOT NULL,
    [UnitId]          VARCHAR (50)    NOT NULL,
    [Quantity]        DECIMAL (18, 2) NOT NULL,
    [UnitCost]        DECIMAL (18, 2) NOT NULL,
    [DiscountPercent] DECIMAL (18, 2) NULL,
    [DiscountAmount]  DECIMAL (18, 2) NULL,
    [TaxPercent]      DECIMAL (18, 2) NULL,
    [TaxAmount]       DECIMAL (18, 2) NULL,
    [TotalAmount]     DECIMAL (18, 2) NOT NULL,
    [Notes]           NVARCHAR (MAX)  NULL,
    [BatchId]         VARCHAR (50)    NULL,
    [SerialId]        VARCHAR (50)    NULL,
    [Active]          BIT             CONSTRAINT [DF_PurchaseItems_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   CONSTRAINT [DF_PurchaseItems_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchaseItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchaseItems_ProductBatches] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProductBatches] ([Id]),
    CONSTRAINT [FK_PurchaseItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_PurchaseItems_ProductSerials] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProductSerials] ([Id]),
    CONSTRAINT [FK_PurchaseItems_Purchases] FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchases] ([Id]),
    CONSTRAINT [FK_PurchaseItems_Units] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[Units] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PurchaseItems_BatchId]
    ON [dbo].[PurchaseItems]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchaseItems_ProductId]
    ON [dbo].[PurchaseItems]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchaseItems_PurchaseId]
    ON [dbo].[PurchaseItems]([PurchaseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchaseItems_UnitId]
    ON [dbo].[PurchaseItems]([UnitId] ASC);

