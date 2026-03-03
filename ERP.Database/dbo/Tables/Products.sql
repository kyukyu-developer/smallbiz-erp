CREATE TABLE [dbo].[Products] (
    [Id]              VARCHAR (50)    NOT NULL,
    [Code]            NVARCHAR (50)   NOT NULL,
    [Name]            NVARCHAR (200)  NOT NULL,
    [Description]     NVARCHAR (1000) NULL,
    [CategoryId]      VARCHAR (50)    NOT NULL,
    [BaseUnitId]      VARCHAR (50)    NOT NULL,
    [MinimumStock]    DECIMAL (18, 2) NULL,
    [MaximumStock]    DECIMAL (18, 2) NULL,
    [ReorderLevel]    DECIMAL (18, 2) NULL,
    [Barcode]         NVARCHAR (100)  NULL,
    [IsBatchTracked]  BIT             NOT NULL,
    [IsSerialTracked] BIT             NOT NULL,
    [Active]          BIT             CONSTRAINT [DF_Products_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [CreatedAt]       DATETIME2 (7)   CONSTRAINT [DF_Products_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       NVARCHAR (50)   NULL,
    [UpdatedAt]       DATETIME2 (7)   NULL,
    [UpdatedBy]       NVARCHAR (50)   NULL,
    [LastAction]      NVARCHAR (50)   NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_Products_Units] FOREIGN KEY ([BaseUnitId]) REFERENCES [dbo].[Units] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Products_BaseUnitId]
    ON [dbo].[Products]([BaseUnitId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Products_CategoryId]
    ON [dbo].[Products]([CategoryId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Products_Code]
    ON [dbo].[Products]([Code] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Products_Name]
    ON [dbo].[Products]([Name] ASC);

