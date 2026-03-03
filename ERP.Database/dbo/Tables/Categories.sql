CREATE TABLE [dbo].[Categories] (
    [Id]               VARCHAR (50)   NOT NULL,
    [Code]             VARCHAR (20)   NOT NULL,
    [Name]             VARCHAR (50)   NOT NULL,
    [ParentCategoryId] VARCHAR (50)   NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [IsUsed]           BIT            NOT NULL,
    [Active]           BIT            CONSTRAINT [DF_Categories_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]        DATETIME2 (7)  CONSTRAINT [DF_Categories_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        NVARCHAR (50)  NULL,
    [UpdatedAt]        DATETIME2 (7)  NULL,
    [UpdatedBy]        NVARCHAR (50)  NULL,
    [LastAction]       NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Categories_ParentCategories] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[Categories] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Categories_ParentCategoryId]
    ON [dbo].[Categories]([ParentCategoryId] ASC);

