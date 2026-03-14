CREATE TABLE [dbo].[ProdCategory] (
    [Id]               VARCHAR (50)   NOT NULL,
    [Code]             VARCHAR (20)   NOT NULL,
    [Name]             VARCHAR (50)   NOT NULL,
    [ParentCategoryId] VARCHAR (50)   NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [IsUsed]           BIT            NOT NULL,
    [Active]           BIT            DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        NVARCHAR (50)  NULL,
    [UpdatedAt]        DATETIME2 (7)  NULL,
    [UpdatedBy]        NVARCHAR (50)  NULL,
    [LastAction]       NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ProdCategory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProdCategory_ParentProdCategory] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[ProdCategory] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdCategory_ParentCategoryId]
    ON [dbo].[ProdCategory]([ParentCategoryId] ASC);

