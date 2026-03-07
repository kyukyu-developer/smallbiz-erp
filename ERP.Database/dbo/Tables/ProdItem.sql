CREATE TABLE [dbo].[ProdItem] (
    [Id]                 VARCHAR (50)    NOT NULL,
    [Code]               NVARCHAR (50)   NOT NULL,
    [Name]               NVARCHAR (200)  NOT NULL,
    [GroupId]            VARCHAR (50)    NULL,
    [CategoryId]         VARCHAR (50)    NOT NULL,
    [BrandId]            VARCHAR (50)    NULL,
    [Description]        NVARCHAR (1000) NULL,
    [BaseUnitId]         VARCHAR (50)    NOT NULL,
    [MinimumStock]       DECIMAL (18, 2) NULL,
    [MaximumStock]       DECIMAL (18, 2) NULL,
    [ReorderLevel]       DECIMAL (18, 2) NULL,
    [Barcode]            NVARCHAR (100)  NULL,
    [TrackType]          INT             NOT NULL,
    [HasVariant]         BIT             NOT NULL,
    [AllowNegativeStock] BIT             NULL,
    [Active]             BIT             CONSTRAINT [DF__ProdItem__Active__440B1D61] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]          DATETIME2 (7)   CONSTRAINT [DF__ProdItem__Create__44FF419A] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          NVARCHAR (50)   NULL,
    [UpdatedAt]          DATETIME2 (7)   NULL,
    [UpdatedBy]          NVARCHAR (50)   NULL,
    [LastAction]         NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProdItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProdItem_ProdBrand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[ProdBrand] ([Id]),
    CONSTRAINT [FK_ProdItem_ProdCategory] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ProdCategory] ([Id]),
    CONSTRAINT [FK_ProdItem_ProdGroup] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[ProdGroup] ([Id]),
    CONSTRAINT [FK_ProdItem_ProdUnit] FOREIGN KEY ([BaseUnitId]) REFERENCES [dbo].[ProdUnit] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProdItem_BaseUnitId]
    ON [dbo].[ProdItem]([BaseUnitId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdItem_CategoryId]
    ON [dbo].[ProdItem]([CategoryId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProdItem_Code]
    ON [dbo].[ProdItem]([Code] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProdItem_Name]
    ON [dbo].[ProdItem]([Name] ASC);

