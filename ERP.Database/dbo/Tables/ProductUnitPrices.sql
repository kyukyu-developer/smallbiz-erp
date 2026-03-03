CREATE TABLE [dbo].[ProductUnitPrices] (
    [Id]         VARCHAR (50)    NOT NULL,
    [ProductId]  VARCHAR (50)    NOT NULL,
    [UnitId]     VARCHAR (50)    NOT NULL,
    [SalePrice]  DECIMAL (18, 2) NOT NULL,
    [Active]     BIT             CONSTRAINT [DF_ProductUnitPrices_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]  DATETIME2 (7)   CONSTRAINT [DF_ProductUnitPrices_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (50)   NULL,
    [UpdatedAt]  DATETIME2 (7)   NULL,
    [UpdatedBy]  NVARCHAR (50)   NULL,
    [LastAction] NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProductUnitPrices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductUnitPrices_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_ProductUnitPrices_Units] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[Units] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductUnitPrices_ProductId]
    ON [dbo].[ProductUnitPrices]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductUnitPrices_UnitId]
    ON [dbo].[ProductUnitPrices]([UnitId] ASC);

