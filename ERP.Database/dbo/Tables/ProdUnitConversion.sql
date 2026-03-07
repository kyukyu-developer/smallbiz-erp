CREATE TABLE [dbo].[ProdUnitConversion] (
    [Id]         VARCHAR (50)    NOT NULL,
    [ProductId]  VARCHAR (50)    NOT NULL,
    [FromUnitId] VARCHAR (50)    NOT NULL,
    [ToUnitId]   VARCHAR (50)    NOT NULL,
    [Factor]     DECIMAL (18, 6) NOT NULL,
    [Active]     BIT             DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (50)   NULL,
    [UpdatedAt]  DATETIME2 (7)   NULL,
    [UpdatedBy]  NVARCHAR (50)   NULL,
    [LastAction] NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProdUnitConversion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProdUnitConversion_ProdItem] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProdItem] ([Id]),
    CONSTRAINT [FK_ProdUnitConversion_ProdUnit] FOREIGN KEY ([FromUnitId]) REFERENCES [dbo].[ProdUnit] ([Id]),
    CONSTRAINT [FK_ProdUnitConversion_ProdUnit1] FOREIGN KEY ([ToUnitId]) REFERENCES [dbo].[ProdUnit] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdUnitConversion_FromUnitId]
    ON [dbo].[ProdUnitConversion]([FromUnitId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProdUnitConversion_ProductId_FromUnitId_ToUnitId]
    ON [dbo].[ProdUnitConversion]([ProductId] ASC, [FromUnitId] ASC, [ToUnitId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdUnitConversion_ToUnitId]
    ON [dbo].[ProdUnitConversion]([ToUnitId] ASC);

