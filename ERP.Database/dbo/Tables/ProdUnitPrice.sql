CREATE TABLE [dbo].[ProdUnitPrice] (
    [Id]         VARCHAR (50)    NOT NULL,
    [ProductId]  VARCHAR (50)    NOT NULL,
    [UnitId]     VARCHAR (50)    NOT NULL,
    [SalePrice]  DECIMAL (18, 2) NOT NULL,
    [Active]     BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (50)   NULL,
    [UpdatedAt]  DATETIME2 (7)   NULL,
    [UpdatedBy]  NVARCHAR (50)   NULL,
    [LastAction] NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProdUnitPrice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProdUnitPrice_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_ProdUnitPrice_ProdUnit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[ProdUnit] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdUnitPrice_ProductId]
    ON [dbo].[ProdUnitPrice]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdUnitPrice_UnitId]
    ON [dbo].[ProdUnitPrice]([UnitId] ASC);

