CREATE TABLE [dbo].[ProdGroup] (
    [Id]          VARCHAR (50)   NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [description] NVARCHAR (255) NULL,
    [Active]      BIT            CONSTRAINT [DF__ProdGroup__Activ__45F365D3] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]   DATETIME2 (7)  CONSTRAINT [DF__ProdGroup__Creat__46E78A0C] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]   DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (50)  NULL,
    [UpdatedBy]   NVARCHAR (50)  NULL,
    [LastAction]  NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ProdGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdGroup_Active]
    ON [dbo].[ProdGroup]([Active] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdGroup_Name]
    ON [dbo].[ProdGroup]([Name] ASC);

