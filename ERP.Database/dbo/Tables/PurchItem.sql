CREATE TABLE [dbo].[PurchItem] (
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
    [Active]          BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_PurchItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchItem_ProdBatch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[ProdBatch] ([Id]),
    CONSTRAINT [FK_PurchItem_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_PurchItem_ProdSerial] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[ProdSerial] ([Id]),
    CONSTRAINT [FK_PurchItem_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id]),
    CONSTRAINT [FK_PurchItem_PurchInvoice] FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[PurchInvoice] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PurchItem_BatchId]
    ON [dbo].[PurchItem]([BatchId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchItem_ProductId]
    ON [dbo].[PurchItem]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchItem_PurchaseId]
    ON [dbo].[PurchItem]([PurchaseId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchItem_SerialId]
    ON [dbo].[PurchItem]([SerialId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PurchItem_UnitId]
    ON [dbo].[PurchItem]([UnitId] ASC);

