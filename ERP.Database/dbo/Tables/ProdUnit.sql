CREATE TABLE [dbo].[ProdUnit] (
    [Id]         VARCHAR (50)  DEFAULT ('') NOT NULL,
    [Name]       VARCHAR (50)  DEFAULT ('') NOT NULL,
    [Symbol]     VARCHAR (50)  DEFAULT ('') NOT NULL,
    [Active]     BIT           DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]  DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (50) NULL,
    [UpdatedAt]  DATETIME2 (7) NULL,
    [UpdatedBy]  NVARCHAR (50) NULL,
    [LastAction] NVARCHAR (50) NULL,
    CONSTRAINT [PK_ProdUnit] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdUnit_Active]
    ON [dbo].[ProdUnit]([Active] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdUnit_Name]
    ON [dbo].[ProdUnit]([Name] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProdUnit_Symbol]
    ON [dbo].[ProdUnit]([Symbol] ASC);

